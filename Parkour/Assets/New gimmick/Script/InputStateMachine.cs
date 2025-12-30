using UnityEngine;

public class InputStateMachine : MonoBehaviour
{
    public enum RunnerState { UP, DOWN, RUNNING }
    public enum phase {enter, stay, exit}

    public RunnerState currentState = RunnerState.RUNNING;
    public bool haltElapsedOneFrame;

    private CharacterController playerCC;
    private Animator playerAnim;
    private AnimatorStateInfo state;
    private float microVib = 0.001f;
    private float duration;

    private float verticalVelocity;
    public float elapsed;
    private float _lockTimer; // The "Solution 1" Lock
    
    public enum Lane { left, middle, right }
    public Lane currentLane = Lane.middle;
    private Lane previousLane = Lane.middle;
    public phase currentPhase;

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

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
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
                playerAnim.SetTrigger("Right");
            }
            else if (direction.x < 0 && currentLane > Lane.left)
            {
                previousLane = currentLane; // Save current as previous
                currentLane--;
                playerAnim.SetTrigger("Left");
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
                        playerAnim.Play("jump", -1, 0f); // Force animation restart
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
        
        // Count down the lock
        if (_lockTimer > 0) _lockTimer -= Time.deltaTime;

        // 3. Passive Reset (Only happens if NOT locked)
        if (currentState != RunnerState.RUNNING && _lockTimer <= 0)
        {
            if (state.normalizedTime >= 1.0f && !playerAnim.IsInTransition(0))
            {
                currentState = RunnerState.RUNNING;
            }
        }
        ApplyStatePhysics();
        ApplyLaneMovement();

        playerCC.Move(move);
        
        CollisionFlags flags = playerCC.Move(move);
        
        // If we hit something on the sides while changing lanes, go back
        if ((flags & CollisionFlags.Sides) != 0)
        {
            currentLane = previousLane;
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
        if (playerCC.isGrounded && currentState != RunnerState.UP)
    {
        jumpCount = 0;
    }
    
    // Safety fallback: if we are grounded and the jump curve is mostly finished
    if (playerCC.isGrounded && currentState == RunnerState.UP && elapsed > 0.1f)
    {
        jumpCount = 0;
    }

        elapsed += Time.deltaTime;
      
        switch (currentState)
        {
            case RunnerState.RUNNING:
                if (playerCC.isGrounded) verticalVelocity = -0.5f;
                else verticalVelocity += -9.81f * Time.deltaTime;
                move.y = verticalVelocity * Time.deltaTime;
                elapsed = 0;
                microVib = -microVib;
                move.x = microVib;
                move.z = microVib;
            break;

            case RunnerState.UP:
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
    