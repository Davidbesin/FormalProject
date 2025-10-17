using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class CharController : MonoBehaviour
{
    public Animator animator;
    public CharacterController characterController;
    private Vector3 rootMotionDelta;
   // [SerializeField] RigidBody rb;

    private GroundManager groundManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        groundManager = GetComponent<GroundManager>();
        animator.applyRootMotion = false;
    }

    void OnAnimatorMove()
    {
        rootMotionDelta = animator.deltaPosition;
        rootMotionDelta.y = 0f;
    }

    void Update()
    {
        animator.applyRootMotion = false; // Check if grounded using GroundManager's public fields
        

        // Apply root motion manually
        if (rootMotionDelta.magnitude > 0.001f)
        {
            characterController.Move(rootMotionDelta);
        }
        
        // Clamp to ground if grounded
        ClampToGround();
    }

    void ClampToGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 2f, groundManager.groundMask))
        {
            float groundY = hit.point.y;
            Vector3 pos = transform.position;

            if (pos.y < groundY)
            {
                pos.y = Mathf.Lerp(pos.y, groundY, 0.5f);
                transform.position = pos;
            }
        }
    }
}