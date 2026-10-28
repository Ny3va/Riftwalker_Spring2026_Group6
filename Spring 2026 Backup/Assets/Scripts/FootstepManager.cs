using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] jumpingClips;
    public AudioClip[] footstepClips;
    public AudioSource audioSource;

    [Range(0, 1)] public float volume = 0.5f;
    [Range(0.1f, 0.5f)] public float pitchRange = 0.1f;

    public void PlayJumping()
    {
        if (jumpingClips.Length == 0) return;
        int index = Random.Range(0, jumpingClips.Length);
        AudioClip clip = jumpingClips[index];
        audioSource.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
        audioSource.PlayOneShot(clip, volume);
    }
    public void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[index];

        audioSource.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);

        audioSource.PlayOneShot(clip, volume);
    }
}