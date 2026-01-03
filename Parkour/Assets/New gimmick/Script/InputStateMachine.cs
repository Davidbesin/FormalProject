using UnityEngine;

public class InputStateMachine : MonoBehaviour
{
     
    public enum RunnerState { UP, DOWN, RUNNING };
    public RunnerState currentState = RunnerState.RUNNING;
    

    private CharacterController playerCC;
    private Animator playerAnim;
    public AnimatorStateInfo state;
    private float microVib = 0.1f;
    private float duration;

    private float verticalVelocity;
    public float elapsed;
    private float _lockTimer; // The "Solution 1" Lock
    
    
    public enum Lane { left, middle, right };
    public Lane currentLane = Lane.middle;
    private Lane previousLane = Lane.middle;

    [Header("Jump Settings")]
    public int jumpCount = 0;
    public int maxJumps = 2; // Set to 2 for Double Jump

    [Header("laneMovement")]
    public float laneWidth = 2f;
    public float laneSmoothTime = 0.12f;
    private float laneVelocity;

    [Header("Curves")]
    public AnimationCurve jumpYCurve;
    public AnimationCurve goLeftCurve;
    public AnimationCurve goRightCurve;

    private CameraFollow camFollow; 

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        camFollow = Camera.main.GetComponent<CameraFollow>(); // Find the camera
    }

    private void OnEnable() => SwipeManager.OnSwipe += HandleSwipe;
    private void OnDisable() => SwipeManager.OnSwipe -= HandleSwipe;

    private void HandleSwipe(Vector2 direction)
    {
        // 1. Reset mechanics immediately
       
        _lockTimer = 0.1f; // Lock for 0.1 seconds (approx 6 frames)

        // 2. Highest Authority: Determine State
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Store the lane before we change it
            Lane targetLane = currentLane;

            if (direction.x > 0 && currentLane < Lane.right)
            {
                previousLane = currentLane; // Save current as previous
                currentLane++;
                if(playerCC.isGrounded){playerAnim.SetTrigger("Right");}
                
            }
            else if (direction.x < 0 && currentLane > Lane.left)
            {
                previousLane = currentLane; // Save current as previous
                currentLane--;
                if(playerCC.isGrounded && !state.IsName("RollStyle")){playerAnim.SetTrigger("Left");}
            }
        }
        else
        {
            if (direction.y > 0)
            { 
                if (jumpCount < maxJumps)
                {
                    currentState = RunnerState.UP;
                    elapsed = 0; // Reset curve progress for the new jump
                    jumpCount++; 
                
                    
                     if (jumpCount > 1) 
                    {
                        // Replaced SetTrigger with Play to force an immediate restart of the state
                        playerAnim.Play("BIG JUMP", 0, 0f); 
                    }
                    else 
                    {
                        playerAnim.SetTrigger("jump");
                    }
                }
            }
            else
            {
                currentState = RunnerState.DOWN;
                playerAnim.SetTrigger("roll");
            }
        }
    }

    void Update()
    {
        state = playerAnim.GetCurrentAnimatorStateInfo(0);

        // CENTRALIZED JUMP RESET
        // Reset jumpCount if on ground, but ignore the first 0.1s of a jump to prevent instant reset
        if (playerCC.isGrounded && (currentState != RunnerState.UP || elapsed > 0.1f))
        {
            jumpCount = 0;
        }

        if (_lockTimer > 0) _lockTimer -= Time.deltaTime;

        // Passive Reset
       
        ApplyStatePhysics();
        ApplyLaneMovement();

        // REMOVED: playerCC.Move(move); (You had this twice, removed the duplicate)
        CollisionFlags flags = playerCC.Move(move);
    
        if ((flags & CollisionFlags.Sides) != 0)
        {
            currentLane = previousLane;
            if(camFollow != null) camFollow.RequestShake();
        }
    }
    
    
    //------------laneMovement
    void ApplyLaneMovement()
    {
        float targetX = ((int)currentLane - 1) * laneWidth;

        float newX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,
            ref laneVelocity,
            laneSmoothTime
        );

        move.x = newX - transform.position.x;
    }


    Vector3 move = Vector3.zero;
    Vector3 delta;
    float t;
    float rightDelta;

    private void ApplyStatePhysics()
    {

        elapsed += Time.deltaTime;
      
        switch (currentState)
        {
            case RunnerState.RUNNING:
                if (playerCC.isGrounded) verticalVelocity = -0.5f;
                else verticalVelocity += -9.81f * Time.deltaTime;
                playerCC.height = 2f;
                playerCC.radius = 0.32f;
                playerCC.center = new Vector3(0, 0.78f, 0);
                move.y = verticalVelocity * Time.deltaTime;
                elapsed = 0;
                microVib = -microVib;
                move.x = microVib;
                move.z = microVib;
            break;

            case RunnerState.UP:
                playerCC.height = 2f;
                playerCC.radius = 0.32f;
                playerCC.center = new Vector3(0, 0.78f, 0);  
                duration = 0.7f;
                float t = Mathf.Clamp01(elapsed / duration);
               // Calculate delta and add it to the main move vector
               float yDelta = jumpYCurve.Evaluate(t) - jumpYCurve.Evaluate(t - (Time.deltaTime / duration));
               move.y = yDelta;
            break;

            case RunnerState.DOWN:
                playerCC.height = 0.81f;
                playerCC.radius = 0.47f;
                playerCC.center = new Vector3(0, 0.33f, 0.21f);
                delta = Vector3.zero;
                move.y = verticalVelocity;
            break;
        }

    }
}
    