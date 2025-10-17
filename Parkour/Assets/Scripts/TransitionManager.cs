using UnityEngine;

public class TransitionManagers : MonoBehaviour
{
    public Animator animator;

    [SerializeField] CharacterController characterController;
    [SerializeField] CharController charController;
    [SerializeField] Rigidbody rb;
    [SerializeField] RbController rbController;

    private bool inTransition = false;

    private string desiredMode = "Character"; // "Character" or "Rigidbody"
    private string appliedMode = "Character";

    void Start()
    {
        ApplyCharacterMode(); // Start with Character mode
    }

    void Update()
    {
        if (animator.IsInTransition(0))
        {
            inTransition = true;

            AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);

            if (nextState.IsName("Run"))
            {
                desiredMode = "Character";
            }
            else if (nextState.IsName("Jump"))
            {
                desiredMode = "Rigidbody";
            }
        }
        else
        {
            inTransition = false;

            AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);

            if (current.IsName("Run"))
                desiredMode = "Character";
            else if (current.IsName("Jump"))
                desiredMode = "Rigidbody";
        }
    }

    void FixedUpdate()
    {
        if (desiredMode != appliedMode)
        {
            if (desiredMode == "Character")
                ApplyCharacterMode();
            else if (desiredMode == "Rigidbody")
                ApplyRigidbodyMode();

            appliedMode = desiredMode;
        }

        if (inTransition)
        {
          //  rb.linearVelocity = Vector3.zero;
          //  rb.angularVelocity = Vector3.zero;
        }
    }

    void ApplyCharacterMode()
    {
        Debug.Log("Applying Character mode on FixedUpdate.");

        rb.isKinematic = true;
        rbController.enabled = false;

        characterController.enabled = true;
        charController.enabled = true;
    }

    void ApplyRigidbodyMode()
    {
        Debug.Log("Applying Rigidbody mode on FixedUpdate.");

        characterController.enabled = false;
        charController.enabled = false;

        rb.isKinematic = false;
        rbController.enabled = true;
    }
}