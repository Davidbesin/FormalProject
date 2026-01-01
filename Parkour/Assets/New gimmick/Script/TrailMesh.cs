using UnityEngine;

public class TrailMesh : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputStateMachine stateMachine;
    [SerializeField] private GameObject trailObject; // The object with the TrailRenderer

    [Header("Settings")]
    [Tooltip("Distance from lane center to consider 'landed'")]
    [SerializeField] private float arrivalThreshold = 0.05f;

    private bool _isMigrating = false;
    private TrailRenderer _trailRenderer;

    void Start()
    {
        if (stateMachine == null) stateMachine = GetComponent<InputStateMachine>();
        
        if (trailObject != null)
        {
            _trailRenderer = trailObject.GetComponent<TrailRenderer>();
            trailObject.SetActive(false);
        }
    }

    void Update()
    {
        if (stateMachine == null || trailObject == null) return;

        // Calculate target X using the same logic as InputStateMachine
        float targetX = ((int)stateMachine.currentLane - 1) * stateMachine.laneWidth;
        float currentX = transform.position.x;

        // Check horizontal distance to target lane
        float distanceToTarget = Mathf.Abs(targetX - currentX);

        if (distanceToTarget > arrivalThreshold)
        {
            // Player has started moving toward a new lane
            if (!_isMigrating)
            {
                _isMigrating = true;
                trailObject.SetActive(true);
            }
        }
        else
        {
            // Player has landed on the lane
            if (_isMigrating)
            {
                _isMigrating = false;
                trailObject.SetActive(false);
                
                
            }
        }
    }
}
