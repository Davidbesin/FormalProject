using UnityEngine;

public class AnimationReset : StateMachineBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   override public void OnStateExit(Animator animator, AnimatorStateInfo state, int layerIndex)
   {
       animator.gameObject.GetComponent<AnimationStateMachine>().elapsed = 0;
   }
}
