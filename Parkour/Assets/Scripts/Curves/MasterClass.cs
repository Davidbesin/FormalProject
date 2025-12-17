using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MasterClass : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string stateName = "Jump";

    [Header("Curves (0..1 time, 0..1 offset factor)")]
    public AnimationCurve forwardCurve; // relative forward progress
    public AnimationCurve yCurve;       // relative vertical progress

    [Header("Timing")]
    public float duration = 1f;

    protected Rigidbody rb;
    protected Vector3 startPos;
    protected Vector3 targetPos;
    protected float elapsed;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        startPos = transform.position;
        elapsed = 0f;

        // Example target (replace with detection later)
        targetPos = startPos + transform.forward * 2f + Vector3.up * 1.5f;
    }

    protected virtual void FixedUpdate()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(stateName))
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // --- Forward (relative) ---
            float forwardFactor = forwardCurve.Evaluate(t);
            float totalForward = Vector3.ProjectOnPlane(targetPos - startPos, Vector3.up).magnitude;
            Vector3 forwardOffset = transform.forward * (forwardFactor * totalForward);

            // --- Vertical (relative) ---
            float yFactor = yCurve.Evaluate(t);
            float heightDiff = targetPos.y - startPos.y;
            float yPos = startPos.y + (yFactor * heightDiff);

            // --- Combine ---
            Vector3 pos = startPos + forwardOffset;
            pos.y = yPos;

            rb.position = pos;
        }
    }
}