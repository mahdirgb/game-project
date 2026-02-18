using UnityEngine;
using System.Collections;

public class DurableCoinRespawn : MonoBehaviour
{
    [Header("Hit Settings")]
    [Tooltip("How many hits before the coin disappears temporarily.")]
    public int maxHits = 1;

    [Tooltip("Alpha after being hit but not yet removed.")]
    [Range(0f, 1f)] public float damagedAlpha = 0.5f;

    [Header("Animation Settings")]
    public float hitScaleMultiplier = 1.2f;
    public float hitAnimationDuration = 0.2f;

    [Header("Respawn Settings")]
    [Tooltip("How long the coin stays invisible before reactivating.")]
    public float respawnDelay = 3f;

    private int currentHits = 0;
    private SpriteRenderer spriteRenderer;
    private Collider2D coinCollider;
    private Vector3 originalScale;

     [Header("Name of the child GameObject to animate")]
    public string childName = "Effect";

    [Header("Animation Settings")]
    public float animationDuration = 1.0f;
    public Vector3 targetScale = Vector3.one;

    [Header("Explode Effect Settings")]
    public Sprite effectSprite;
    private GameObject subObject;
    private Transform subTransform;
    private SpriteRenderer subSpriteRenderer;

    public float EffectanimationDuration = 1.0f;
    public float sortingOrder = 10; // optional: draw on top

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coinCollider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
    }
    private void Awake()
    {
        subObject = new GameObject(childName);
        subObject.transform.SetParent(transform);
        subObject.transform.localPosition = Vector3.zero;
        subObject.transform.localRotation = Quaternion.identity;

        // Add SpriteRenderer
        subSpriteRenderer = subObject.AddComponent<SpriteRenderer>();
        subSpriteRenderer.sprite = effectSprite;
        subSpriteRenderer.sortingOrder = (int)sortingOrder;

        // Store transform for animation
        subTransform = subObject.transform;

        // Start inactive
        subObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        currentHits++;
        ScoreManager.Instance.AddScore(2);
        StopAllCoroutines();
        StartCoroutine(PlayHitAnimation());
        PlayEffect();

        if (currentHits >= maxHits + 2)
        {
            StartCoroutine(RespawnAfterDelay());
        }
        else
        {
            // Set semi-transparent
            Color c = spriteRenderer.color;
            c.a = damagedAlpha;
            spriteRenderer.color = c;
        }
    }

    private IEnumerator PlayHitAnimation()
    {
        float t = 0f;
        Vector3 targetScale = originalScale * hitScaleMultiplier;

        // Scale up
        while (t < hitAnimationDuration)
        {
            t += Time.deltaTime;
            float progress = t / hitAnimationDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, progress);
            yield return null;
        }

        // Scale down
        t = 0f;
        while (t < hitAnimationDuration)
        {
            t += Time.deltaTime;
            float progress = t / hitAnimationDuration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    private IEnumerator RespawnAfterDelay()
    {
        // Make invisible and disable collision
        spriteRenderer.enabled = false;
        coinCollider.enabled = false;
        int randomValue = Random.Range(4, 11);
        yield return new WaitForSeconds(randomValue);

        // Reactivate
        currentHits = 0;
        spriteRenderer.enabled = true;
        coinCollider.enabled = true;

        // Restore full opacity
        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;
    }
    public void PlayEffect()
    {
        if (subObject != null && spriteRenderer != null)
            StartCoroutine(AnimateSubObject());
    }

    private IEnumerator AnimateSubObject()
    {
        subObject.SetActive(true);
        subTransform.localScale = Vector3.zero;

        // Set initial alpha to 1
        Color color = subSpriteRenderer.color;
        color.a = 1f;
        subSpriteRenderer.color = color;

        float time = 0f;

        while (time < EffectanimationDuration)
        {
            float t = time / EffectanimationDuration;

            // Scale
            subTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            // Fade
            color.a = Mathf.Lerp(1f, 0f, t);
            subSpriteRenderer.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final state
        subTransform.localScale = Vector3.one;
        color.a = 0f;
        subSpriteRenderer.color = color;

        subObject.SetActive(false);
    }
}
