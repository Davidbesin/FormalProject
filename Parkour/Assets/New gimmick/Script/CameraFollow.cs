using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;// Adjust in Inspector
    public float sideSmoothSpeed = 5f; // How fast it follows lane changes
    public float upSmoothSpeed = 5f;   // How fast it follows jumps
     public float yOffset = 2.5f; 

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Smooth Sideways (X)
        float targetX = player.position.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, sideSmoothSpeed * Time.deltaTime);

        // 2. Smooth Vertical (Y) + Manual Offset
        // We add yOffset to the player's current height
        float targetY = player.position.y + yOffset;
        float newY = Mathf.Lerp(transform.position.y, targetY, upSmoothSpeed * Time.deltaTime);

        // 3. Locked Forward (Z)
        // No Lerp here: the camera stays exactly 'zDistance' behind the player

        // Apply the final position
        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
