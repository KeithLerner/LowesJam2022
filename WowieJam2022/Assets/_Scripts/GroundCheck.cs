using UnityEngine;

[System.Serializable]
public class GroundCheck
{
    [SerializeField] private Transform origin;
    public Transform Origin { get { return origin; } }

    [SerializeReference] private float distanceThreshold = .15f;
    public float DistanceThreshold { get { return distanceThreshold; } }

    private bool storedGroundedValue;
    public bool StoredGroundedValue { get { return storedGroundedValue; } }

    private float storedLastTimeGrounded;
    public float StoredLastTimeGrounded { get { return storedLastTimeGrounded; } }
    public float TimeSinceLastGrounded { get { return Time.time - storedLastTimeGrounded; } }
    
    /// <summary>
    /// Called when the ground is touched again.
    /// </summary>
    public event System.Action OnGrounded;

    /// <summary>
    /// Called when the ground is touched.
    /// </summary>
    public event System.Action WhenGrounded;

    public GroundCheck(Transform origin, float threshold)
    {
        this.origin = origin;
        this.distanceThreshold = threshold;
    }

    public GroundCheck(GroundCheck gc)
    {
        this.origin = gc.Origin;
        this.distanceThreshold = gc.distanceThreshold;
    }

    public void SetOrigin(Transform origin)
    {
        this.origin = origin;
    }

    public void SetThreshold(float threshold)
    {
        this.distanceThreshold = threshold;
    }

    public bool IsGrounded()
    {
        // Check if we are grounded now.
        // Update storedGroundedValue.
        storedGroundedValue = Physics2D.Raycast(origin.position, Vector2.down, distanceThreshold);
        if (storedGroundedValue)
            storedLastTimeGrounded = Time.time;
        return storedGroundedValue;
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
        Debug.DrawLine(origin.position, origin.position - Vector3.up * distanceThreshold, 
            storedGroundedValue ? Color.green : Color.red);
    }
}
