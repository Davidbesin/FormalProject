using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class RbController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animator;

    [Tooltip("Upward impulse applied when jump starts.")]
    public float jumpImpulse = 6f;

    [Tooltip("Optional horizontal impulse for diagonal jump.")]
    public float horizontalImpulse = 4f;

    [Tooltip("Use camera-relative direction for horizontal impulse.")]
    public bool useCameraRelative = true;

    private bool applyJump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Update()
    {
        if (animator.IsInTransition(0))
        {
            var next = animator.GetNextAnimatorStateInfo(0);
            if (next.IsName("Jump"))
            {
                applyJump = true;
            }
        }
        else
        {
            var cur = animator.GetCurrentAnimatorStateInfo(0);
            if (cur.IsName("Jump") && cur.normalizedTime < 0.05f)
            {
                applyJump = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (!applyJump) return;
        applyJump = false;

        if (float.IsNaN(jumpImpulse) || float.IsInfinity(jumpImpulse)) return;

        Vector3 direction = transform.forward;

        if (useCameraRelative && Camera.main != null)
        {
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0f;
            direction = camForward.normalized;
        }

        Vector3 impulse = direction * horizontalImpulse + Vector3.up * jumpImpulse;

        const float maxImpulse = 50f;
        if (impulse.magnitude > maxImpulse)
        {
            impulse = impulse.normalized * maxImpulse;
        }

        rb.AddForce(impulse, ForceMode.Impulse);
        ClampVelocity(rb, 100f);
    }

    static void ClampVelocity(Rigidbody r, float maxSpeed)
    {
        Vector3 v = r.linearVelocity;

        if (float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z) ||
            float.IsInfinity(v.x) || float.IsInfinity(v.y) || float.IsInfinity(v.z))
        {
            r.linearVelocity = Vector3.zero;
            return;
        }

        if (v.sqrMagnitude > maxSpeed * maxSpeed)
        {
            r.linearVelocity = v.normalized * maxSpeed;
        }
    }
}