using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [Header("Ground Check")]
    public bool isGrounded;
    public bool isGrounded1;
    public bool isGrounded2;

    [SerializeField] public Transform groundCheck1;
    [SerializeField] public Transform groundCheck2;

    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    void FixedUpdate()
    {
        // Check ground at both points
        isGrounded1 = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask);
        isGrounded2 = Physics.CheckSphere(groundCheck2.position, groundDistance, groundMask);

        // Combine into a single grounded flag
        isGrounded = isGrounded1 || isGrounded2;
    }

    /// <summary>
    /// Attempts to get the Y position of the ground directly below the object.
    /// </summary>
    public bool TryGetGroundY(out float groundY)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 2f, groundMask))
        {
            groundY = hit.point.y;
            return true;
        }

        groundY = 0f;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck1.position, groundDistance);
        }
        if (groundCheck2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck2.position, groundDistance);
        }
    }
}