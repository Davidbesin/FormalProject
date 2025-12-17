using UnityEngine;

public class jessenewscript : MonoBehaviour
{

    public Camera varCamera;
    public CharacterController cc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 moveVar = new Vector3 (1, 0, 0);  
        cc.Move(moveVar);
    }
}
