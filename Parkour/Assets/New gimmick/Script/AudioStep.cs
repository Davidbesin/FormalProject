using UnityEngine;

public class AudioStep : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepSounds; // Array for variety

    private void OnTriggerEnter(Collider other)
    {
        // Only play if the surface is tagged as Ground and character is moving
        if (other.CompareTag("Ground"))
        {
            PlayRandomFootstep();
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepSounds.Length > 0)
        {
            // Pick a random clip for more realism
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
