using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Rigidbody parentRigidbody;

    [Header("Settings")]
    public bool useRootMotion = true;


    // Internal flag to ensure first-time setup happens only once
    private bool hasInitialized = false;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (parentRigidbody == null) parentRigidbody = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        // First-time setup: sync transforms and apply initial toggle
        

        // Apply root motion toggle on subsequent frames
        animator.applyRootMotion = useRootMotion;


    }
  
  
    public void SwitchToRootMotion()
    {
        useRootMotion = true;
       
    }

    public void SwitchToPhysics()
    {
        useRootMotion = false;
    }
}