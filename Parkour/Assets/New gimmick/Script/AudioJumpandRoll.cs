using UnityEngine;

public class AudioJumpandRoll : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] rollSounds; // Array for variety
    private InputStateMachine iSM;

    void Start()
    {
        // Only play if the surface is tagged as Ground and character is moving
        iSM = GetComponent<InputStateMachine>();
    }

    void Update()
    {
        // Only play if the surface is tagged as Ground and character is moving
        PlayRandomFootstep();
    }

    void PlayRandomFootstep()
    {
        if (rollSounds.Length > 0)
        {
            // Pick a random clip for more realism
            AudioClip clip = rollSounds[Random.Range(0, rollSounds.Length)];
            
        }
    }
}
