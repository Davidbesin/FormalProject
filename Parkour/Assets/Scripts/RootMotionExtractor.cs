using UnityEngine;

public class RootMotionExtractor : MonoBehaviour
{
    private Animator animator;

    public Vector3 extractedRootMotion;
    public Vector3 motionForController;
    public Vector3 motionForRigidbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorMove()
    {
        extractedRootMotion = animator.deltaPosition;
        Debug.Log("Raw Root Motion: " + animator.deltaPosition);

        Vector3 horizontalMotion = extractedRootMotion;
        horizontalMotion.y = 0f;

        // Split motion for CharacterController and Rigidbody
        motionForController = transform.forward * horizontalMotion.magnitude;
        motionForRigidbody = horizontalMotion; // raw horizontal motion for physics
    }

    public void ClearMotion()
    {
        extractedRootMotion = Vector3.zero;
        motionForController = Vector3.zero;
        motionForRigidbody = Vector3.zero;
    }
}