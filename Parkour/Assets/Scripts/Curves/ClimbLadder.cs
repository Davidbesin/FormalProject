using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClimbLadder
: MonoBehaviour
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
            Vector3 targetPos; //= startPos + startForward * forwardDist;
            targetPos = startPos + transform.up * yOffset; 
            ;
            //targetPos.x = 0f; targetPos.z = 0f;
       

            rb.MovePosition(targetPos);
        }

        
    }

    public void ResetAnimationProgress()
    {
        elapsed = 0f;
        Debug.Log("Animation progress reset to 0.");
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