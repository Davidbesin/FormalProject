using UnityEngine;

public class rbc : MonoBehaviour
{
    [Header("Components")]
    public CharacterController characterController;
    public Rigidbody rb;
    [SerializeField] PlayerInput input;

    [Header("Ground Check")]
    public bool isGrounded;
    public bool isGrounded1;
    public bool isGrounded2;
    [SerializeField] private Transform groundCheck1;
    [SerializeField] private Transform groundCheck2;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    public RootMotionExtractor rootMove;
    public bool autoGroundCheck = true;

    [Header("Flag (toggle this)")]
    public bool groundedFlag = true;

    bool lastGroundedState;

    void Awake()
    {
        characterController = characterController ?? GetComponent<CharacterController>();
        rb = rb ?? GetComponent<Rigidbody>();
        rb.isKinematic = true;
        lastGroundedState = groundedFlag;
    }

    void Update()
    {
        if (isGrounded)
        {
            characterController.enabled = true;

            if (rootMove != null)
            {
                Vector3 motion = rootMove.motionForController;
                characterController.Move(motion * 10); // Adjust multiplier as needed
            }
        }
        else
        {
            characterController.enabled = false;
        }
    }

    void FixedUpdate()
    {
        isGrounded1 = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask);
        isGrounded2 = Physics.CheckSphere(groundCheck2.position, groundDistance, groundMask);
        isGrounded = isGrounded1 || isGrounded2;

        rb.isKinematic = isGrounded;

        if (isGrounded)
        {
            ClampToGround();

            if (rootMove != null)
            {
                rb.MovePosition(rb.position + rootMove.motionForRigidbody * 10); // Adjust multiplier as needed
                rootMove.ClearMotion();
            }
        }
    }

    void ClampToGround()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 2f, groundMask))
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
}