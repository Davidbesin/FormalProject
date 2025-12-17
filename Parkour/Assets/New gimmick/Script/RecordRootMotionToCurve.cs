using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class RootMotionCurveRecorder : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string targetStateName = "Jump";
    public float recordDuration = 1f;

    [Header("Output Curves")]
    public AnimationCurve forwardCurve;
    public AnimationCurve yCurve;

    private List<Keyframe> forwardKeys = new List<Keyframe>();
    private List<Keyframe> yKeys = new List<Keyframe>();

    private Vector3 startPos;
    private float elapsed;
    private bool recording = true;
    private bool baked = false;

    void OnEnable()
    {
        startPos = transform.position;
        elapsed = 0f;
        recording = true;
        baked = false;

        forwardKeys.Clear();
        yKeys.Clear();
    }

    void OnAnimatorMove()
    {
        if (!recording || baked) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(targetStateName)) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / recordDuration);

        // Get root motion delta
        Vector3 delta = animator.deltaPosition;
        Quaternion deltaRot = animator.deltaRotation;

        // Apply root motion to move the character
        transform.position += delta;
        transform.rotation *= deltaRot;

        // Record position relative to starting point
        Vector3 localOffset = transform.position - startPos;
        float forwardDist = Vector3.Dot(localOffset, transform.forward);
        float yPos = transform.position.y;

        forwardKeys.Add(new Keyframe(t, forwardDist));
        yKeys.Add(new Keyframe(t, yPos));

        if (elapsed >= recordDuration)
        {
            forwardCurve = new AnimationCurve(forwardKeys.ToArray());
            yCurve = new AnimationCurve(yKeys.ToArray());
            baked = true;
            Debug.Log("✅ Root motion curves recorded.");
        }
    }
}