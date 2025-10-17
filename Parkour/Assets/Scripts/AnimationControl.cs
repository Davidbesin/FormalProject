using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;

    // Flags you can set from other scripts
    public bool isRunning;
    public bool isFalling;

    void Update()
    {
        // Push flags into Animator
        animator.SetBool("isRunning", isRunning);
        // Falling can still use a bool if needed
        animator.SetBool("isFalling", isFalling);
    }

    // Example helper methods to trigger states
    public void TriggerRun()
    {
        ResetFlags();
        isRunning = true;
    }

    public void TriggerJump()
    {
        ResetFlags();
        animator.SetTrigger("Jump"); // Use Trigger instead of bool
    }

    public void ResetFlags()
    {
        isRunning = false;
        isFalling = false;
    }
}