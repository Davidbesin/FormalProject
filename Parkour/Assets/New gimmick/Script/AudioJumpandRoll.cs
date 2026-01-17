using UnityEngine;

public class AudioJumpandRoll : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] rollSounds; // Array for variety
    public GameObject player;
    private AnimatorStateInfo state;

    void Start()
    {
        // Only play if the surface is tagged as Ground and character is moving
        
        state = player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        // Only play if the surface is tagged as Ground and character is moving
       if (state.IsName("RollStyle"))
       {    
           PlayRandomFootstep();
       }
    }

    void PlayRandomFootstep()
    {
        if (rollSounds.Length > 0)
        {
            // Pick a random clip for more realism
            AudioClip clip = rollSounds[Random.Range(0, rollSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
        //https://sketchfab.com/3d-models/coin-9fe04caac11a4bf68c5e46df08adabb5
    }
}
