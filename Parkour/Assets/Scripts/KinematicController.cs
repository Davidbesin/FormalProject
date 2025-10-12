using UnityEngine;

public class KinematicController : MonoBehaviour
{
    public Rigidbody rb;
    

    public void KinematicTrue()
     
    {
          rb.isKinematic = true;
    }
      public void KinematicFalse()
     
    {
          rb.isKinematic = false;
    }
}