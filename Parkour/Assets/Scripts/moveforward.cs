using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class moveforward: MonoBehaviour
{
    public Animator animator;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

   void OnAnimatorMove()
{
    Vector3 rootMotion = animator.deltaPosition;

    // Log root motion and controller movement
    Debug.Log("Root Motion: " + rootMotion);

    Vector3 before = transform.position;

    controller.Move(rootMotion);

    Vector3 after = transform.position;
    Vector3 actualMove = after - before;

    Debug.Log("CharacterController moved: " + actualMove);
}
}