using UnityEngine;


public class charController : MonoBehaviour
{
    [Header("Components")]
    public CharacterController characterController;
    public Rigidbody rb;
    [SerializeField] PlayerInput input;

    [Header("Ground Check")]
    public bool isGrounded;                          // Combined grounded result
    public bool isGrounded1;                         // Ground check 1
    public bool isGrounded2;
    [SerializeField] private Transform groundCheck1;
    [SerializeField] private Transform groundCheck2;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public RootMotionExtractor rootMove;
    public bool autoGroundCheck = true; // when true, this overwrites groundedFlag each FixedUpdate

    [Header("Flag (toggle this)")]
    public bool groundedFlag = true;    // set this flag externally to request grounded / airborne

    // internal state to detect changes
    bool lastGroundedState;

    void Awake()
    {
        characterController = characterController ?? GetComponent<CharacterController>();
        rb = rb ?? GetComponent<Rigidbody>();
        rb.isKinematic = true; // start grounded
        lastGroundedState = groundedFlag;
        // ensure initial application of the flag
       
    }

    void FixedUpdate()
    {
        // optional automatic ground detection that overwrites the flag
        
        isGrounded1 = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask);
        isGrounded2 = Physics.CheckSphere(groundCheck2.position, groundDistance, groundMask);
        isGrounded =  isGrounded1 || isGrounded2;
        // if flag changed, apply the switch
        Controller();
        // Apply root motion every physics frame
        ApplyRootMotion();

    }

    // centralized switch logic called only internally when the flag changes
    void Controller()
    { 
        if (isGrounded)
        {
            // switch to CharacterController (grounded)
            rb.isKinematic = true;
            characterController.enabled = true;
        }
        else
        {
            // switch to Rigidbody (airborne)
            characterController.enabled = false;
            rb.isKinematic = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        
        if (groundCheck1 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck1.position, groundDistance);
        }
        if (groundCheck2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck2.position, groundDistance);
        }
    
    }

    //Rootmotion
   void ApplyRootMotion()
   {
        if (isGrounded && rootMove != null)
        {
            Vector3 motion = rootMove.motion;

            // Apply root motion using CharacterController
            characterController.Move(motion * 1000f);

            // Reset the root motion after applying
            rootMove.ClearMotion();
        }
   }

}