using UnityEngine;

public class MoveSlowly : MonoBehaviour
{
    [Tooltip("How fast the world moves toward the player")]
    public float scrollSpeed = 10f;

    void Update()
    {
        // Move the entire environment backward (Negative Z)
        // transform.forward represents the world's forward direction
        transform.Translate(-Vector3.forward * scrollSpeed * Time.deltaTime);
    }
}