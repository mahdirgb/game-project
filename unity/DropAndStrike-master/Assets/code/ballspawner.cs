using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public float spawnY = 5f;
    public TMP_Text ballsText;
    public int numBalls = 10;

    void Start()
    {
        StartCoroutine(AddValueRoutine());
    }

    IEnumerator AddValueRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            numBalls += 5;
            UpdateUI();

        }
    }
     private void UpdateUI()
    {
        if (ballsText != null)
        {
            ballsText.text = numBalls.ToString();
        }
    }

    void Update()
    {
        // Handle mouse input (for Editor, PC)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnBallAtPosition(Mouse.current.position.ReadValue());
        }

        // Handle touch input (for mobile)
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            SpawnBallAtPosition(touchPosition);
        }
    }

    void SpawnBallAtPosition(Vector2 screenPosition)
    {
        if (numBalls <= 0)
            return;
        numBalls--;
        UpdateUI();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        Vector3 spawnPosition = new Vector3(worldPosition.x, spawnY, 0f);
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }
}
