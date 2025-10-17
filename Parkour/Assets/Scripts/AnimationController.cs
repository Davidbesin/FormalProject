using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public Animator anim;
    private Transform rootBone;
    [SerializeField] private Vector3 lockedRootPosition;
    [SerializeField] private AnimationControl control;
     [SerializeField] private GroundManager gm;

    void Start()
    {
        rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
        if (rootBone == null)
        {
            Debug.LogError("Root bone (Hips) not found.");
        }
    }

   void Update()
   {
        if (input.jumpButtonPressed && (gm.isGrounded || gm.isGrounded1 || gm.isGrounded2))
        {
            control.TriggerJump();
            input.jumpButtonPressed = false; // Consume the input
            Debug.Log("Jumping");
        }
        else if (gm.isGrounded || gm.isGrounded1 || gm.isGrounded2)
        {
            control.TriggerRun();
        }
   }      /*  if (rootBone != null)
        {
            lockedRootPosition = new Vector3(0, rootBone.localPosition.y, 0);
            rootBone.localPosition = lockedRootPosition;
        }
        */

}
