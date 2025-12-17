using UnityEngine;

public class AnimationStateMachine : MonoBehaviour
{
    // Components
    private CharacterController playerCC;
    private Vector3 charMove; 
    private AnimatorStateInfo state;
    public float duration = 1f;
    public float elapsed;
    public Animator playerAnim;
    public FixedTouchField touchField;

    // Gravity settings
    public float gravity = -9.81f;
    private float verticalVelocity;

    // flags
    private bool swipeUp;
    private bool swipeDown;
    private bool swipeLeft;
    public bool swipeRight; 

    // ENUM
    public enum Lane{
        left,
        middle,
        right  
    }

    public Lane currentLane = Lane.middle;

    [Header("Jump Curves")]
    public AnimationCurve jumpYCurve;       // relative Y offset

    [Header("Short Roll Curves")]
    public AnimationCurve shortRollYCurve;  // relative Y offset

    [Header("ChangeLaneLeft")]
    public AnimationCurve goLeftCurve;      // relative left distance

    [Header("ChangeLaneRight")]
    public AnimationCurve goRightCurve;     // relative right distance
    

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    void Update()
    { 
        // Swipe detection
        if (touchField.Pressed)
        {
            Vector2 swipe = touchField.TouchDist;

            if (swipe.magnitude > 50f)
            {
                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                    {
                        if (currentLane < Lane.right) swipeRight = true;
                        else swipeRight = false;

                        swipeUp = false; swipeLeft = false; swipeDown = false;
                    }
                    else
                    {
                        if (currentLane > Lane.left) swipeLeft = true;
                        else swipeLeft = false;
                        swipeUp = false; swipeRight = false; swipeDown = false;
                    }  
                }
                else
                {
                    if (swipe.y > 0)
                    {
                        swipeRight = false; swipeUp = true; swipeLeft = false; swipeDown = false;
                    }
                    else
                    {
                        swipeRight = false; swipeUp = false; swipeLeft = false; swipeDown = true;
                    }
                }

                touchField.TouchDist = Vector2.zero;
            }
        }

        // Trigger actions
        if (swipeUp && !touchField.Pressed && playerCC.isGrounded){Jump();}
        else if (swipeDown  && !touchField.Pressed){Roll();}
        else if (swipeLeft && !touchField.Pressed && currentLane > Lane.left){GoLeft(); currentLane--;}
        else if (swipeRight && !touchField.Pressed && currentLane < Lane.right){GoRight(); currentLane++;}

        state = playerAnim.GetCurrentAnimatorStateInfo(0);

        // Run state (no forward motion)
        if (state.IsName("Run")) 
        {
            playerCC.height = 1.85f;
            playerCC.radius = 0.32f;
            playerCC.center = new Vector3(0, 0.78f, 0); 

            charMove = Vector3.zero;

            if (playerCC.isGrounded && verticalVelocity < 0)
                verticalVelocity = -0.5f;
            else
                verticalVelocity += gravity * Time.deltaTime;

            charMove.y = verticalVelocity * Time.deltaTime;
            playerCC.Move(charMove);
        }

        // Jump state
        else if (state.IsName("Jump"))
        {            
            playerCC.height = 0.81f;
            playerCC.radius = 0.47f;
            playerCC.center = new Vector3(0, 0.33f, 0.21f);

            elapsed += Time.deltaTime;
            duration = 0.6f;
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
            duration = 1f;
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
    
    // Swipe methods
    public void Jump(){playerAnim.SetTrigger("jump"); swipeUp = false;}
    public void Roll(){playerAnim.SetTrigger("roll"); swipeDown = false;}
    public void GoLeft(){playerAnim.SetTrigger("Left"); swipeLeft = false;}
    public void GoRight(){playerAnim.SetTrigger("Right"); swipeRight = false;} 

    private bool stumble; // check if it once in a short time

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag ("Obstacle"))
        {
            if (!stumble)
            {
                playerAnim.SetTrigger("Stumble");
            }

            else 
            {
                 playerAnim.SetTrigger("Stumble");
            }
        }
    }
}