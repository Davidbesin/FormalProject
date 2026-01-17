using UnityEngine;

public class FunkyDream : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Inside your method:
        if (Random.Range(0, 5000) == 0) 
        {
            anim.SetTrigger("YourTrigger");
        }   
    }
}
