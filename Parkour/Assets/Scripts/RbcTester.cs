using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RootMotionJumpForce : MonoBehaviour
{
    public Animator animator;
    public float forceMultiplier = 10f; // Controls the "umph"
    public bool applyRootMotionForce = true;

    private Rigidbody rb;
    private Vector3 previousRootPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousRootPosition = animator.rootPosition;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!applyRootMotionForce) return;

        // Calculate root motion delta
        Vector3 currentRootPosition = animator.rootPosition;
        Vector3 delta = currentRootPosition - previousRootPosition;

        // Focus on Y (up) and Z (forward) only
        Vector3 jumpVector = new Vector3(0f, delta.y, delta.z).normalized;

        // Apply force
        rb.AddForce(jumpVector * forceMultiplier, ForceMode.Impulse);

        // Update previous position
        previousRootPosition = currentRootPosition;

        // Optional: Debug visualization
        Debug.DrawRay(rb.position, jumpVector * forceMultiplier, Color.green, 0.2f);
    }
}