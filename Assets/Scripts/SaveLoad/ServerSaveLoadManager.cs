using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ServerSaveLoadManager<T> : ISaveLoadManager<T>
{
    private string _serverUrl = "http://localhost:3000/api";
    private string _playerId = "player123"; // Player ID mặc định, nên được cấu hình lại

    public void SetServerUrl(string url)
    {
        _serverUrl = url;
    }

    public void SetPlayerId(string playerId)
    {
        _playerId = playerId;
    }

    // Interface bắt buộc — không truyền playerId, dùng _playerId nội bộ
    public void Save(T data, System.Action<bool> onComplete = null)
    {
        CoroutineRunner.Instance.StartCoroutine(SaveCoroutine(data, onComplete));
    }

    // Interface bắt buộc — key chính là playerId
    public void Load(string key, System.Action<T> onLoaded)
    {
        CoroutineRunner.Instance.StartCoroutine(LoadCoroutine(key, onLoaded));
    }

    private IEnumerator SaveCoroutine(T data, System.Action<bool> onComplete)
    {
        // Gửi JSON dạng: { playerId, gameData }
        SaveWrapper wrapper = new SaveWrapper { playerId = _playerId, gameData = data };
        string json = JsonUtility.ToJson(wrapper);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(_serverUrl + "/saveGame", "POST"))
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

    private IEnumerator LoadCoroutine(string playerId, System.Action<T> onLoaded)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{_serverUrl}/loadGame/{playerId}"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                LoadWrapper wrapper = JsonUtility.FromJson<LoadWrapper>(json);
                onLoaded?.Invoke(wrapper.gameData);
                Debug.Log("✅ Load success");
            }
            else
            {
                Debug.LogError("❌ Load failed: " + request.error);
                onLoaded?.Invoke(default);
            }
        }
    }

    [System.Serializable]
    private class SaveWrapper
    {
        public string playerId;
        public T gameData;
    }

    [System.Serializable]
    private class LoadWrapper
    {
        public T gameData;
    }
}
