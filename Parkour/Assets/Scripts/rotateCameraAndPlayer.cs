using UnityEngine;

public class rotateCameraAndPlayer : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    public Transform rotateBody; // Reference to the player body (for horizontal rotation)
    public Transform cameraTransform;
    public float sensitivity = 0.1f; // Adjust sensitivity to your liking
    private float xRotation = 0f;
    public CameraLook _CameraLook;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateBody.Rotate(Vector3.up * input.touchLeftRight * sensitivity);
        // Vertical rotation (X-axis)
         if (_CameraLook != null)
        {
            _CameraLook.LockAxis = new Vector2(input.touchLeftRight, input.touchUpDown);
        }
    }
}
