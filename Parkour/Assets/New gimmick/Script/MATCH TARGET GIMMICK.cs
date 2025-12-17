using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MatchTargetGimmick : MonoBehaviour
{
    Animator anim;
    public float startTime;
    public float endTime;
    public GameObject chestTarget;
    public GameObject handtarget;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("JumpUp"))
        {
            MatchTargetR();
            Aim();
        }
    }

    void MatchTargetR()
    {
        anim.MatchTarget(handtarget.transform.position, handtarget.transform.rotation, AvatarTarget.RightHand,
                         new MatchTargetWeightMask(Vector3.one, 0), startTime, endTime);
    }

    void Aim()
    {
        if (Vector3.Distance(chestTarget.transform.position, transform.position) < 100)
        {
            Vector3 direction = chestTarget.transform.position - transform.position;
            direction.y = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction), 5f);
        }
    }
}