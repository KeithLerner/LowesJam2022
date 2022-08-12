using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Deceleration;

        public MovementSettings(float maxSpeed, float acceleration, float deceleration)
        {
            this.MaxSpeed = maxSpeed;
            this.Acceleration = acceleration;
            this.Deceleration = deceleration;
        }
    }

    [Header("Movement Settings")]
    [SerializeField] private float coyoteTime = .25f;
    [SerializeField] private float gravityMagnitude = 10;
    [SerializeField] private MovementSettings groundSettings = new MovementSettings(7, 14, 14);
    [SerializeField] private MovementSettings airSettings = new MovementSettings(7, 20, 14);
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private Jumps jumps;

    // Inputs
    private Vector2 inputs;
    private Vector2 targetVelocity;
    private Vector2 wishDirection;
    private float lastUpMovementInputValue;

    // Outputs
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gather Inputs
        inputs.x = InputManager.instance.horizontalMoveAxis;
        inputs.y = InputManager.instance.verticalMoveAxis;
        QueueJump();        

        // Get grounded
        if (groundCheck.IsGrounded())
        {
            if (!groundCheck.StoredGroundedValue) groundCheck.DoOnGrounded();
            else groundCheck.DoWhenGrounded();

            // Ground Move
            GroundMove(Time.deltaTime);

            // Apply Friction
            Friction(Time.deltaTime);
        }
        else
        {
            // Air Move
            AirMove(Time.deltaTime);

            // Apply Gravity
            if (Time.time > groundCheck.StoredLastTimeGrounded + coyoteTime)
                Gravity(Time.deltaTime);
        }

        // Jumping
        if (jumps.JumpQueued)
        {
            DoJump();
        }

        // Debug Visual
        groundCheck.DrawGroundCheck();
    }

    private void FixedUpdate()
    {
        // Apply movement
        rb.velocity = targetVelocity;
    }

    public void GroundMove(float physicsTimeInterval)
    {
        float wishspeed = wishDirection.magnitude * groundSettings.MaxSpeed;
        targetVelocity.y = 0;
        Accelerate(groundSettings.Acceleration, wishspeed, physicsTimeInterval);
    }

    public void AirMove(float physicsTimeInterval)
    {

    }

    // Does full jump sequence
    private void DoJump()
    {
        targetVelocity.y = Mathf.Sqrt(2 * gravityMagnitude * jumps.CharacterHeight * jumps.JumpHeight);
        jumps.Jump();
        jumps.JumpQueued = false;
    }

    // Queues Jump
    private void QueueJump()
    {
        if (jumps.AutoBunnyHop)
        {
            // Queue Jump
            inputs.y = (inputs.y > 0 || InputManager.instance.jumpHeld ||
                InputManager.instance.jumpPressed) ? 1 : 0;

        }
        else
        {
            if (lastUpMovementInputValue > 0)
            {
                // Queue Jump
                inputs.y = (InputManager.instance.jumpPressed) ? 1 : 0;
            }
            else
            {
                // Queue Jump
                inputs.y = (inputs.y > 0 || InputManager.instance.jumpPressed) ? 1 : 0;
            }
        }
        jumps.JumpQueued = inputs.y > 0;
        lastUpMovementInputValue = inputs.y;
    }

    // TF2 or Source style acceleration
    private void Accelerate(float acceleration, float max_velocity, float physicsTimeInterval)
    {
        float projVel = Vector2.Dot(targetVelocity, wishDirection); // Vector projection of Current velocity onto accelDir.
        float accelVel = acceleration * physicsTimeInterval; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        targetVelocity += wishDirection * accelVel;
    }

    // Apply Friction
    private void Friction(float physicsTimeInterval)
    {
        float speed = Mathf.Abs(targetVelocity.x);
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * groundSettings.Deceleration * physicsTimeInterval;
            targetVelocity.x *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }
    }

    private void Gravity(float physicsTimeInterval)
    {
        targetVelocity -= Vector2.down * gravityMagnitude * physicsTimeInterval;
    }
}
