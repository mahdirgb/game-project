using UnityEngine;
using System.Collections;
using TMPro;

public class bucket : MonoBehaviour
{
    public AudioClip soundClip;
    public AudioSource sfxSource;

    public TMP_Text scoreTxt;

    private int score = 5;

    [Header("Hit Animation")]
    public Color targetColor = Color.red;
    public Vector3 targetScale = Vector3.one * 1.2f;
    public float animationDuration = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalScale;
    private Coroutine animationCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the bucket object.");
        }
        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        scoreTxt.text = score.ToString();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            ScoreManager.Instance.AddScore(score);
            score = Random.Range(4, 11);
            scoreTxt.text = score.ToString();
            PlaySound();
            PlayHitAnimation();
        }
    }

    private void PlaySound()
    {
        if (soundClip == null)
        {
            Debug.LogWarning("Missing soundClip!");
            return;
        }
        sfxSource.PlayOneShot(soundClip);
    }

    public void PlayHitAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(HitAnimationCoroutine());
    }

    private IEnumerator HitAnimationCoroutine()
    {
        float halfDuration = animationDuration / 2f;
        float time = 0f;

        // Animate from original to target color & scale
        while (time < halfDuration)
        {
            float t = time / halfDuration;
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, t);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure target state reached
        spriteRenderer.color = targetColor;
        transform.localScale = targetScale;

        time = 0f;
        // Animate back from target to original color & scale
        while (time < halfDuration)
        {
            float t = time / halfDuration;
            spriteRenderer.color = Color.Lerp(targetColor, originalColor, t);
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure original state restored
        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;

        animationCoroutine = null;
    }
}
