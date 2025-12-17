using UnityEngine;

public class DetectiionSystem : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float detectionRange = 2f; 
    [SerializeField] private Collider detectionCollider;

    private Collider nearestObstacle;

    // This is the only thing exposed: the offset value
    public float yOffset;

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
            nearestObstacle = other;
    }

    void OnTriggerExit(Collider other)
    {
        if (other == nearestObstacle)
        {
            nearestObstacle = null;
            yOffset = 0f;
        }
    }

    void Update()
    {
        if (nearestObstacle != null)
        {
            // Cancel if wall blocks
            if (Physics.Raycast(transform.position, transform.forward, detectionRange, wallLayer))
            {
                nearestObstacle = null;
                yOffset = 0f;
                return;
            }

            // Confirm obstacle directly ahead
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, obstacleLayer))
            {
                if (hit.collider == nearestObstacle)
                {
                    float obstacleTop = hit.collider.bounds.max.y;
                    float playerBaseY = transform.position.y;

                    // Store offset directly
                    yOffset = obstacleTop - playerBaseY;
                }
            }
        }
    }
}