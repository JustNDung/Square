using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerSaveLoadManager<T> : ISaveLoadManager<T>
{
    private string serverUrl = "http://localhost:3000/api"; // Thay bằng server thực
    
    public void SetServerUrl(string url)
    {
        serverUrl = url;
    }

    public void Save(T data, System.Action<bool> onComplete = null)
    {
        CoroutineRunner.Instance.StartCoroutine(SaveCoroutine(data, onComplete));
    }

    public void Load(string key, System.Action<T> onLoaded)
    {
        CoroutineRunner.Instance.StartCoroutine(LoadCoroutine(key, onLoaded));
    }

    private IEnumerator SaveCoroutine(T data, System.Action<bool> onComplete)
    {
        string json = JsonUtility.ToJson(data);
        UnityWebRequest www = UnityWebRequest.Put(serverUrl + "/save", json);
        www.method = UnityWebRequest.kHttpVerbPOST;
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        bool success = www.result == UnityWebRequest.Result.Success;
        if (!success)
            Debug.LogError("Save failed: " + www.error);

        onComplete?.Invoke(success);
    }

    private IEnumerator LoadCoroutine(string key, System.Action<T> onLoaded)
    {
        UnityWebRequest www = UnityWebRequest.Get(serverUrl + "/load/" + key);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            T data = JsonUtility.FromJson<T>(json);
            onLoaded?.Invoke(data);
        }
        else
        {
            Debug.LogError("Load failed: " + www.error);
            onLoaded?.Invoke(default);
        }
    }
}

public class GameDataSaveLoadManager : ServerSaveLoadManager<GameData>
{
    // Có thể thêm các phương thức đặc thù cho GameData nếu cần
}