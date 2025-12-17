using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VaultByCurve : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string jumpStateName = "Jump";

    [Header("Vault Motion Curves")]
    public AnimationCurve forwardCurve; // forward distance in meters
    public AnimationCurve yCurve;       // world Y offset
    public float duration = 2f;

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 startForward;
    private float elapsed;

    [Header("IK Targets")]
    [SerializeField] private Transform Lefthold;
    [SerializeField] private Transform Righthold;

    [Header("IK Curves")]
    [SerializeField] private AnimationCurve leftHandIKCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve rightHandIKCurve = AnimationCurve.Linear(0, 0, 1, 1);

    // --- IK timing ---
    private bool itsTime = false;   // flag controlled by animation events
    private float ikElapsed = 0f;   // clock for IK blending
    private float ikDuration = 1f;  // normalized to 1 over this window

    // --- Base curves (saved from Inspector) ---
    private AnimationCurve baseForwardCurve;
    private AnimationCurve baseYCurve;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Save Inspector curves as base copies
        baseForwardCurve = new AnimationCurve(forwardCurve.keys);
        baseYCurve = new AnimationCurve(yCurve.keys);
    }

    void OnEnable()
    {
        startPos = transform.position;
        startForward = transform.forward;
        elapsed = 0f;

        // Debug log curves
        LogCurveKeyframes(forwardCurve, "ForwardCurve");
        LogCurveKeyframes(yCurve, "YCurve");
    }

    void FixedUpdate()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // Example of how to change values dynamically:
        // SetForwardValue(1, 5f);   // change middle frame value
        // SetYValue(2, 2f);         // change Y curve’s third keyframe

        if (state.IsName(jumpStateName))
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Evaluate vault curves
            float forwardDist = forwardCurve.Evaluate(t);
            float yOffset = yCurve.Evaluate(t);

            // Compute target position
            Vector3 targetPos = startPos + startForward * forwardDist;
            targetPos.y = startPos.y + yOffset;

            rb.MovePosition(targetPos);
        }

        // Advance IK clock if active
        if (itsTime)
        {
            ikElapsed += Time.fixedDeltaTime;
        }
    }

    // --- Animation Event Functions ---
    public void EnableIKForVault()
    {
        itsTime = true;
        ikElapsed = 0f;
        ikDuration = duration; // or set a custom IK duration
    }

    public void DisableIKForVault()
    {
        itsTime = false;
    }

    // --- IK Handling ---
    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(jumpStateName)) return;
        if (!itsTime) return;

        // Normalize IK time [0,1]
        float ikT = Mathf.Clamp01(ikElapsed / ikDuration);

        // Evaluate curves
        float leftWeight = leftHandIKCurve.Evaluate(ikT);
        float rightWeight = rightHandIKCurve.Evaluate(ikT);

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

    // --- Debugging helper ---
    private void LogCurveKeyframes(AnimationCurve curve, string curveName)
    {
        if (curve == null)
        {
            Debug.LogWarning($"{curveName} is null!");
            return;
        }

        Debug.Log($"--- {curveName} Keyframes ---");
        foreach (var key in curve.keys)
        {
            Debug.Log($"{curveName} -> time: {key.time}, value: {key.value}, inTangent: {key.inTangent}, outTangent: {key.outTangent}");
        }
    }

    // --- Dynamic editing methods ---
    public void SetForwardValue(int index, float newValue)
    {
        var keys = forwardCurve.keys;
        if (index < 0 || index >= keys.Length) return;

        keys[index].value = newValue;   // assign new value
        forwardCurve.keys = keys;       // apply back
    }

    public void SetYValue(int index, float newValue)
    {
        var keys = yCurve.keys;
        if (index < 0 || index >= keys.Length) return;

        keys[index].value = newValue;
        yCurve.keys = keys;
    }

    // --- Reset back to Inspector base ---
    public void ResetCurves()
    {
        forwardCurve = new AnimationCurve(baseForwardCurve.keys);
        yCurve = new AnimationCurve(baseYCurve.keys);
    }
}