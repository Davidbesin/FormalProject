using UnityEngine;

public class SimpleLagFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 18f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            followSpeed * Time.deltaTime
        );

        transform.rotation = target.rotation;
    }
}