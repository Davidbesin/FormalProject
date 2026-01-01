using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float sideSmoothSpeed = 5f; 
    public float upSmoothSpeed = 5f;   
    public float yOffset = 2.5f;
    public float zOffset = 2.5f;


    [Header("Shake Settings")]
    public AnimationCurve shakeCurve; // Define a "sawtooth" or "vibration" curve here
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.3f;
    private float shakeTimer = 0;

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Smooth Sideways (X)
        float targetX = player.position.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, sideSmoothSpeed * Time.deltaTime);

        // 2. Smooth Vertical (Y)
        float targetY = player.position.y + yOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, upSmoothSpeed * Time.deltaTime);

        // 3. Shake Logic
        Vector3 shakeOffset = Vector3.zero;
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            // Use curve to determine magnitude over time
            float curveValue = shakeCurve.Evaluate(1f - (shakeTimer / shakeDuration));
            // Random shake multiplied by curve magnitude
            shakeOffset = Random.insideUnitSphere * curveValue * shakeIntensity;
        }

      

        // Apply position + shake
        transform.position = new Vector3(newX, newY, player.position.z - zOffset) + shakeOffset;
    }

    public void RequestShake()
    {
        shakeTimer = shakeDuration;
    }
}
