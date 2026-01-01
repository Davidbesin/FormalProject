using UnityEngine;

public class JumpVFX : MonoBehaviour
{
    public ParticleSystem m_ParticleSystem;
    public InputStateMachine iSM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Start()
   {
       m_ParticleSystem.Stop();
   }
   void Update()
   {
       if (iSM.currentLane == InputStateMachine.Lane.left)
       {
            m_ParticleSystem.transform.position = new Vector3(-2f, m_ParticleSystem.transform.position.y, m_ParticleSystem.transform.position.z);
       }

       else if (iSM.currentLane == InputStateMachine.Lane.middle)
       {
            m_ParticleSystem.transform.position =  new Vector3(0f, m_ParticleSystem.transform.position.y, m_ParticleSystem.transform.position.z);
       }

       else if (iSM.currentLane == InputStateMachine.Lane.right)
       {
            m_ParticleSystem.transform.position =  new Vector3(2f, m_ParticleSystem.transform.position.y, m_ParticleSystem.transform.position.z);
       }

    

   }
    public void StartJumpVFX()
   {
       m_ParticleSystem.Play();
   }

   public void StopJumpVFX()
   {
       m_ParticleSystem.Stop();
   }
}
