using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpByCurve : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string jumpStateName = "Jump";

    [Header("Jump Curves")]
    public AnimationCurve forwardCurve; // relative forward distance
    public AnimationCurve yCurve;       // relative Y offset
    public float duration = 1f;

    private Rigidbody rb;
    private Vector3 startPos;
    private float elapsed;

    [Header("IK Targets")]
    [SerializeField] private Transform Lefthold;
    [SerializeField] private Transform Righthold;

    private bool itsTime = false; // controlled by animation events

    [Header("IK Curves")]
    [SerializeField] private AnimationCurve leftHandIKCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve rightHandIKCurve = AnimationCurve.Linear(0, 0, 1, 1);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        startPos = transform.position;
        elapsed = 0f;
    }

    void FixedUpdate()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(jumpStateName))
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Evaluate curves
            float forwardDist = forwardCurve.Evaluate(t);
            float yOffset = yCurve.Evaluate(t);

            // Compute target
            Vector3 targetPos = startPos + transform.forward * forwardDist;
            targetPos.y = startPos.y + yOffset;

            rb.position = targetPos;
        }
    }

    // --- Animation Event Functions ---
    public void EnableIK()
    {
        itsTime = true;
    }

    public void DisableIK()
    {
        itsTime = false;
    }

    // --- IK Handling ---
    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(jumpStateName)) return;

        // Use the same normalized time as the jump
        float t = Mathf.Clamp01(elapsed / duration);

        // Evaluate curves
        float leftWeight = itsTime ? leftHandIKCurve.Evaluate(t) : 0f;
        float rightWeight = itsTime ? rightHandIKCurve.Evaluate(t) : 0f;

        // Apply IK
        if (Lefthold != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, Lefthold.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, Lefthold.rotation);
        }

        if (Righthold != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, Righthold.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, Righthold.rotation);
        }
    }
}