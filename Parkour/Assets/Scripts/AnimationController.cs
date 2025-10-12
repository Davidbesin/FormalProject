using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public Animator anim;
    private Transform rootBone;
    [SerializeField] private Vector3 lockedRootPosition;

    void Start()
    {
        rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
        if (rootBone == null)
        {
            Debug.LogError("Root bone (Hips) not found.");
        }
    }

    void FixedUpdate()
    {
        if (input.jump)
        {
            anim.SetTrigger("Jump");
        }

        if (rootBone != null)
        {
            lockedRootPosition = new Vector3(0, rootBone.localPosition.y, 0);
            rootBone.localPosition = lockedRootPosition;
        }
    }
}
