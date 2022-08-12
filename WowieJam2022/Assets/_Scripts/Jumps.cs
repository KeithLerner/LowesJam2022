using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jumps
{
    [Header("Jump Variables")]
    [SerializeField, Tooltip("Measured in character height")] private float jumpHeight = .5f;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float jumpCoolDown = 0;
    private bool jumpQueued;

    [Header("Control Variables")]
    [SerializeField, Tooltip("Automatically jump when holding jump input")] private bool autoBunnyHop = false;


    // Tracking Variables
    private float timeOfLastJump;
    private Transform feet, head;
    private int jumpsRemaining;

    // Getters
    public bool CanJump { get { return Time.time >= timeOfLastJump + jumpCoolDown && jumpsRemaining > 0; } }
    public float CharacterHeight { get { return Vector3.Distance(feet.position, head.position); } }
    public float JumpHeight { get { return jumpHeight; } set { jumpHeight = value; } }
    public int MaxJumps { get { return maxJumps; } set { maxJumps = value; } }
    public float JumpCoolDown { get { return jumpCoolDown; } set { jumpCoolDown = value; } }
    public bool JumpQueued { get { return jumpQueued; } set { jumpQueued = value; } }
    public bool AutoBunnyHop { get { return autoBunnyHop; } set { autoBunnyHop = value; } }
    public int JumpsRemaining { get { return jumpsRemaining; } set { jumpsRemaining = value; } }
    public float TimeOfLastJump { get { return timeOfLastJump; } }

    public Jumps(Transform feet, Transform head, int maxJumps = 1, float jumpHeight = .5f, float jumpCoolDown = 0)
    {
        this.feet = feet;
        this.head = head;
        MaxJumps = maxJumps;
        JumpHeight = jumpHeight;
        JumpCoolDown = jumpCoolDown;
        Init();
    }

    public void Init(Transform feet = null, Transform head = null)
    {
        this.feet = feet;
        this.head = head; 
        JumpsRemaining = MaxJumps;
        timeOfLastJump = 0;
    }

    public void Jump()
    {
        JumpsRemaining--;
        timeOfLastJump = Time.time;
    }

    public void OnGrounded()
    {
        JumpsRemaining = MaxJumps;
        timeOfLastJump = -1;
    }

    public void WhenGrounded()
    {
        JumpsRemaining = MaxJumps;
    }
}
