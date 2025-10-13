using UnityEngine;

public class RbController : MonoBehaviour
{
    [Header("Ground Check")]
    public bool isGrounded;
    public bool isGrounded1;
    public bool isGrounded2;
    [SerializeField] private Transform groundCheck1;
    [SerializeField] private Transform groundCheck2;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
