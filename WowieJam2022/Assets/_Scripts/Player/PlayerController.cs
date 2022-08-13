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

        public MovementSettings(float maxSpeed, float acceleration)
        {
            this.MaxSpeed = maxSpeed;
            this.Acceleration = acceleration;
        }
    }

    [Header("Body")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform feet;
    public float CharacterHeight { get { return Vector3.Distance(feet.position, head.position); } }

    [Header("Movement Settings")]
    [SerializeField] private float coyoteTime = .25f;
    [SerializeField] private float gravityMagnitude = 20;
    [SerializeField, Range(0, 1)] private float friction = .8f;
    [SerializeField] private MovementSettings groundSettings = new MovementSettings(7, 14);
    [SerializeField] private MovementSettings airSettings = new MovementSettings(7, 20);
    
    [Header("Jump Settings")]
    [SerializeField, Tooltip("Measured in character height")] private float jumpHeight = .5f;
    private bool jumpQueued;
    private float timeOfLastJump;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float distanceThreshold = .15f;
    public bool isGrounded;
    public bool storedGroundedValue = false;
    private float storedLastTimeGrounded;
    private float TimeSinceLastGrounded { get { return Time.time - storedLastTimeGrounded; } }

    /// <summary>
    /// Called when the ground is touched again.
    /// </summary>
    public event System.Action OnGrounded;
    
    /// <summary>
    /// Called when the ground is touched.
    /// </summary>
    public event System.Action WhenGrounded;


    // Inputs
    private Vector2 inputs;
    private Vector2 targetVelocity;
    private float wishDirection;
    private float storedTimeLastCanJump;

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
        wishDirection = inputs.x;

        // Prepare Movements
        if (isGrounded)
        {
            if (!storedGroundedValue) DoOnGrounded();
            else DoWhenGrounded();

            storedLastTimeGrounded = Time.time;

            // Ground Move
            GroundMove(Time.deltaTime);

            // Jumping
            if (jumpQueued)
            {
                DoJump();
            }
        }
        else
        {
            // Air Move
            AirMove(Time.deltaTime);

            // Gravity
            Gravity(Time.deltaTime);
        }

        // Debug Visual
        DrawGroundCheck();
    }

    private void FixedUpdate()
    {
        storedGroundedValue = isGrounded;
        isGrounded = false;
        RaycastHit2D hit = Physics2D.BoxCast(feet.position, new Vector2(.45f, .25f), 0, Vector2.down);
        if (hit && hit.collider.gameObject != gameObject)
        {
            isGrounded = true;
        }
        // Apply movement
        rb.velocity = targetVelocity;
    }

    public void GroundMove(float physicsTimeInterval)
    {
        targetVelocity.y = 0;
        if (inputs.x == 0)
        {
            // Apply friction
            targetVelocity.x *= friction;
        }
        else
        {
            targetVelocity.x = wishDirection * groundSettings.Acceleration * physicsTimeInterval;
        }
        ClampVelocity(groundSettings.MaxSpeed);
    }

    public void AirMove(float physicsTimeInterval)
    {
        targetVelocity.x += wishDirection * airSettings.Acceleration * physicsTimeInterval;
        ClampVelocity(airSettings.MaxSpeed);
    }

    public void ClampVelocity(float maxSpeed)
    {
        targetVelocity.x = Mathf.Clamp(targetVelocity.x, -maxSpeed, maxSpeed);
    }

    // Does full jump sequence
    private void DoJump()
    {
        targetVelocity.y = Mathf.Sqrt(2 * gravityMagnitude * CharacterHeight * jumpHeight);
        timeOfLastJump = Time.time;
        jumpQueued = false;
    }

    // Queues Jump
    private void QueueJump()
    {
        jumpQueued = TimeSinceLastGrounded < coyoteTime && inputs.y > 0;
    }

    // Applies Extra Gravity
    private void Gravity(float physicsTimeInterval)
    {
        targetVelocity.y -= gravityMagnitude * physicsTimeInterval;
    }

    public void DoOnGrounded()
    {
        OnGrounded?.Invoke();
    }
    public void DoWhenGrounded()
    {
        WhenGrounded?.Invoke();
    }

    public void DrawGroundCheck()
    {
        Debug.DrawLine(feet.position, feet.position - Vector3.up * distanceThreshold,
            isGrounded ? Color.green : Color.red);
    }

}
