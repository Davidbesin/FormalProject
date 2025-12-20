using UnityEngine;

public class InputStateMachine : MonoBehaviour
{
        private enum RunnerState {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };

    private CharacterController playerCC;
    private Animator playerAnim;
    private AnimatorStateInfo state;

    private Vector3 charMove;
    private float duration = 1f;
    public float elapsed;
    private float microVib = 0.001f;

    public float gravity = -9.81f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
