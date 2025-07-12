using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private string _targetScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Gọi từ bất kỳ đâu để chuyển scene
    public void LoadScene(string sceneName)
    {
        _targetScene = sceneName;
        StartCoroutine(LoadTargetScene());
        //SceneManager.LoadScene("LoadingScene"); // scene trung gian
    }

    /// Chạy trong LoadingScene
    public IEnumerator LoadTargetScene()
    {
        yield return new WaitForSeconds(0.2f); // delay nhỏ

        AsyncOperation op = SceneManager.LoadSceneAsync(_targetScene);
        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            // Gọi UI update loading bar nếu có
            yield return null;
        }
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}