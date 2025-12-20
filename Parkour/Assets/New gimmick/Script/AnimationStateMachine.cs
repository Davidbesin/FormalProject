using UnityEngine;

public class AnimationStateMachine : MonoBehaviour
{
    private CharacterController playerCC;
    private Animator playerAnim;
    private AnimatorStateInfo state;

    private Vector3 charMove;
    private float duration = 1f;
    public float elapsed;
    private float microVib = 0.001f;

    public float gravity = -9.81f;
    private float verticalVelocity;

    public FixedTouchField touchField;

    public enum Lane { left, middle, right }
    public Lane currentLane = Lane.middle;

    [Header("Jump Curves")]
    public AnimationCurve jumpYCurve;
    [Header("Short Roll Curves")]
    public AnimationCurve shortRollYCurve;
    [Header("ChangeLaneLeft")]
    public AnimationCurve goLeftCurve;
    [Header("ChangeLaneRight")]
    public AnimationCurve goRightCurve;

    private bool stumble;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();

        // ✅ Subscribe to swipe events
        touchField.OnSwipeUp += HandleSwipeUp;
        touchField.OnSwipeDown += HandleSwipeDown;
        touchField.OnSwipeLeft += HandleSwipeLeft;
        touchField.OnSwipeRight += HandleSwipeRight;
    }

    // -------------------
    // Swipe Handlers
    // -------------------
    private void HandleSwipeUp()
    {
        if (playerCC.isGrounded) Jump();
    }

    private void HandleSwipeDown()
    {
        Roll();
    }

    private void HandleSwipeLeft()
    {
        if (currentLane > Lane.left)
        {
            GoLeft();
            currentLane--;
        }
    }

    private void HandleSwipeRight()
    {
        if (currentLane < Lane.right)
        {
            GoRight();
            currentLane++;
        }
    }

    // -------------------
    // Actions
    // -------------------
    public void Jump()
    {
        playerAnim.SetTrigger("jump");
        elapsed = 0f;
    }

    public void Roll()
    {
        playerAnim.SetTrigger("roll");
        elapsed = 0f;
    }

    public void GoLeft()
    {
        playerAnim.SetTrigger("Left");
        elapsed = 0f;
    }

    public void GoRight()
    {
        playerAnim.SetTrigger("Right");
        elapsed = 0f;
    }

    // -------------------
    // Physics & Animation
    // -------------------
    void LateUpdate()
    {
        // ✅ Still need LateUpdate for physics & animation state progression
        state = playerAnim.GetCurrentAnimatorStateInfo(0);

        // Run state
        if (state.IsName("Run"))
        {
            playerCC.height = 1.85f;
            playerCC.radius = 0.32f;
            playerCC.center = new Vector3(0, 0.78f, 0);

            microVib = -microVib;
            charMove = Vector3.zero;

            if (playerCC.isGrounded && verticalVelocity < 0)
                verticalVelocity = -0.5f;
            else
                verticalVelocity += gravity * Time.deltaTime;

            charMove.y = verticalVelocity * Time.deltaTime;
            charMove.x = microVib;
            charMove.z = microVib;

            playerCC.Move(charMove);
        }

        // Jump state
        else if (state.IsName("Jump"))
        {
            playerCC.height = 0.81f;
            playerCC.radius = 0.47f;
            playerCC.center = new Vector3(0, 0.33f, 0.21f);

            elapsed += Time.deltaTime;
            duration = 0.7f;
            float t = Mathf.Clamp01(elapsed / duration);

            float yDelta = jumpYCurve.Evaluate(t) - jumpYCurve.Evaluate(t - (Time.deltaTime / duration));
            Vector3 delta = Vector3.up * yDelta;
            playerCC.Move(delta);
        }

        // Lane left
        else if (state.IsName("Left"))
        {
            playerCC.height = 1.85f;
            playerCC.radius = 0.33f;
            playerCC.center = new Vector3(0, 0.78f, 0);

            elapsed += Time.deltaTime;
            duration = 0.33f;
            float t = Mathf.Clamp01(elapsed / duration);

            float rightDelta = goLeftCurve.Evaluate(t) - goLeftCurve.Evaluate(t - (Time.deltaTime / duration));
            Vector3 delta = transform.right * -rightDelta;
            playerCC.Move(delta);
        }

        // Lane right
        else if (state.IsName("Right"))
        {
            playerCC.height = 1.85f;
            playerCC.radius = 0.33f;
            playerCC.center = new Vector3(0, 0.78f, 0);

            elapsed += Time.deltaTime;
            duration = 0.33f;
            float t = Mathf.Clamp01(elapsed / duration);

            float rightDelta = goRightCurve.Evaluate(t) - goRightCurve.Evaluate(t - (Time.deltaTime / duration));
            Vector3 delta = transform.right * rightDelta;
            playerCC.Move(delta);
        }

        // Roll state
        else if (state.IsName("RollStyle"))
        {
            playerCC.height = 0.81f;
            playerCC.radius = 0.47f;
            playerCC.center = new Vector3(0, 0.33f, 0.21f);

            Vector3 delta = Vector3.zero;
            delta.y = verticalVelocity;
            playerCC.Move(delta);
        }

        else if (!playerCC.isGrounded && elapsed >= 1)
        {
            verticalVelocity += gravity * Time.deltaTime;
            charMove.y = verticalVelocity * Time.deltaTime;
        }
    }

    // -------------------
    // Collision
    // -------------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle"))
        {
            playerAnim.SetTrigger("Stumble");
        }
    }
}