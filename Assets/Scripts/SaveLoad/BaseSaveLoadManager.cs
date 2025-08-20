using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public abstract class BaseSaveLoadManager<T> : ISaveLoadManager<T>
{
    protected string _baseUrl;

    public BaseSaveLoadManager(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public virtual void Save(T data, System.Action<bool> onComplete = null)
    {
        CoroutineRunner.Instance.StartCoroutine(PostCoroutine(data, onComplete));
    }

    public virtual void Load(string key, System.Action<T> onLoaded)
    {
        CoroutineRunner.Instance.StartCoroutine(GetCoroutine(key, onLoaded));
    }

    protected virtual IEnumerator PostCoroutine(T data, System.Action<bool> onComplete)
    {
        string json = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(_baseUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            bool success = request.result == UnityWebRequest.Result.Success;
            if (!success)
                Debug.LogError("❌ Save failed: " + request.error);
            else
                Debug.Log("✅ Save success");

            onComplete?.Invoke(success);
        }
    }

    protected virtual IEnumerator GetCoroutine(string key, System.Action<T> onLoaded)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{_baseUrl}/{key}"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                T result = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onLoaded?.Invoke(result);
                Debug.Log("✅ Load success");
            }
            else
            {
                Debug.LogError("❌ Load failed: " + request.error);
                onLoaded?.Invoke(default);
            }
        }
    }
}
