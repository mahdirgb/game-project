using UnityEngine;

public class SpriteOpacityToggler : MonoBehaviour
{
    [Header("Opacity Settings")]
    [Range(0f, 1f)]
    public float visibleAlpha = 1f;

    [Range(0f, 1f)]
    public float hiddenAlpha = 0f;

    [Header("Timing Settings")]
    [Tooltip("Initial delay before starting the toggle cycle.")]
    public float initialDelay = 0f;

    [Tooltip("Time to stay visible before hiding.")]
    public float visibleDuration = 1f;

    [Tooltip("Time to stay hidden before showing.")]
    public float hiddenDuration = 1f;

    private SpriteRenderer spriteRenderer;
    private bool isVisible = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteOpacityToggler: No SpriteRenderer found on GameObject.");
            enabled = false;
            return;
        }

        // Start the toggle loop after the initial delay
        Invoke(nameof(StartToggleLoop), initialDelay);
    }

    private void StartToggleLoop()
    {
        StartCoroutine(ToggleLoop());
    }

    private System.Collections.IEnumerator ToggleLoop()
    {
        while (true)
        {
            SetAlpha(isVisible ? visibleAlpha : hiddenAlpha);
            float waitTime = isVisible ? visibleDuration : hiddenDuration;
            isVisible = !isVisible;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
