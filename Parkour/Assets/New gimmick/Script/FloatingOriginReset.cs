using UnityEngine;

public class FloatingOriginReset : MonoBehaviour
{
    [Header("Settings")]
    public float threshold = 2000f; // Distance before reset
    public Transform playerTransform;
    public Transform worldParent; // The parent of all your chunks/obstacles

    private CharacterController playerCC;

    void Start()
    {
        if (playerTransform != null)
            playerCC = playerTransform.GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        // Check if player has moved too far on the Z axis
        if (playerTransform.position.z > threshold)
        {
            ResetWorld();
        }
    }

    void ResetWorld()
    {
        // 1. Calculate the offset (how far we are from center)
        Vector3 offset = new Vector3(0, 0, playerTransform.position.z);

        // 2. IMPORTANT: Disable the CharacterController before teleporting
        // This prevents the physics engine from "fighting" the manual position change
        if (playerCC != null) playerCC.enabled = false;

        // 3. Move the player back to center
        playerTransform.position -= offset;

        // 4. Move the World Parent back by the same amount
        // This keeps the player and the world in the same relative spots
        worldParent.position -= offset;

        // 5. Re-enable the CharacterController
        if (playerCC != null) playerCC.enabled = true;

        Debug.Log("World Origin Reset: Shifting " + offset.z + " units back to center.");
    }
}
