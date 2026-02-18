using UnityEngine;
using System.Collections;

public class SpriteLightBulb : MonoBehaviour
{
    [Header("State Values")]
    [Range(0f, 1f)]
    public float onAlpha = 1f;

    [Range(0f, 1f)]
    public float offAlpha = 0.7f;

    public Vector3 onScale = Vector3.one;
    public Vector3 offScale = new Vector3(0.7f, 0.7f, 0.7f);

    [Header("Timing Settings")]
    [Tooltip("Time the sprite stays fully ON.")]
    public float onDuration = 0.5f;

    [Tooltip("Time the sprite stays fully OFF.")]
    public float offDuration = 0.5f;

    [Tooltip("Time it takes to transition between states.")]
    public float transitionTime = 0.3f;

    [Tooltip("Delay before the loop starts.")]
    public float initialDelay = 0f;

    private SpriteRenderer spriteRenderer;
    private bool isOn = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteLightBulb: No SpriteRenderer attached.");
            enabled = false;
            return;
        }

        Invoke(nameof(StartLoop), initialDelay);
    }

    private void StartLoop()
    {
        StartCoroutine(LightLoop());
    }

    private IEnumerator LightLoop()
    {
        while (true)
        {
            // Toggle state
            isOn = !isOn;

            float startAlpha = spriteRenderer.color.a;
            float targetAlpha = isOn ? onAlpha : offAlpha;

            Vector3 startScale = transform.localScale;
            Vector3 targetScale = isOn ? onScale : offScale;

            float t = 0f;

            // Smooth transition
            while (t < 1f)
            {
                t += Time.deltaTime / transitionTime;
                float smoothed = Mathf.SmoothStep(0f, 1f, t);

                // Interpolate alpha
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(startAlpha, targetAlpha, smoothed);
                spriteRenderer.color = color;

                // Interpolate scale
                transform.localScale = Vector3.Lerp(startScale, targetScale, smoothed);

                yield return null;
            }

            // Wait for duration depending on current state
            yield return new WaitForSeconds(isOn ? onDuration : offDuration);
        }
    }
}
