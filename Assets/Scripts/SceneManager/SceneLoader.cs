using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {   
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadTargetScene(sceneName, onLoaded));
    }

    private IEnumerator LoadTargetScene(string sceneName, Action onLoaded)
    {
        yield return new WaitForSeconds(0.2f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            // update loading bar nếu cần
            yield return null;
        }

        // đợi 1 frame để tất cả Awake/Start chạy xong
        yield return null;

        onLoaded?.Invoke();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}