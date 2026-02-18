using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlaySoundOnBallCollision : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip soundClip;

    private float minPitch = 0.85f;
    private float maxPitch = 1.15f;


    private AudioSource sfxSource;

    void Awake()
    {
        // Add a new hidden AudioSource for this component
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            PlaySoundWithRandomPitch();
        }
    }

    private void PlaySoundWithRandomPitch()
    {
        if (soundClip == null)
        {
            Debug.LogWarning("Missing soundClip!");
            return;
        }
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(soundClip);
    }
}
