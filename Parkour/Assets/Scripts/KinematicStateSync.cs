using UnityEngine;

public class KinematicStateSync : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public KinematicController kinematicController;

    [Header("Animation Settings")]
    public string targetStateName = "Jump"; // Name of the animation state to track

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (kinematicController == null) kinematicController = GetComponentInParent<KinematicController>();
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Layer index hardcoded to 0

        if (stateInfo.IsName(targetStateName))
        {
             kinematicController.KinematicTrue(); // Animation is done
        }
        else
        {
           
            kinematicController.KinematicFalse(); // Animation is playing
        }
    }
}