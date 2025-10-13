using UnityEngine;

public class RootMotionExtractor : MonoBehaviour
{
    private Animator animator;
    public Vector3 extractedRootMotion;
    public Vector3 motion;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        // Extract root motion from the Animator
        extractedRootMotion = animator.deltaPosition;
        Debug.Log("Raw Root Motion: " + animator.deltaPosition);

        // Process it: remove vertical drift and align with forward direction
        Vector3 horizontalMotion = extractedRootMotion;
        horizontalMotion.y = 0f;
        motion = transform.forward * horizontalMotion.magnitude;
    }

    public void ClearMotion()
    {
        extractedRootMotion = Vector3.zero;
        motion = Vector3.zero;
    }
}