using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // Optional
    public string sceneName;
    private void Start()
    {
        LoadScene(sceneName);
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Update progress bar (optional)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            // Wait until loading is done (at 90%), then allow activation
            if (operation.progress >= 0.9f)
            {
                // Optionally wait for user input or delay
                yield return new WaitForSeconds(5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}