using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using UnityEngine.Serialization;

public class ServerSaveLoadManager<T> : ISaveLoadManager<T> where T : GameData
{
    private string _serverUrl = "http://localhost:3000/api";
    private string _playerId = "player123";

    public void SetServerUrl(string url) => _serverUrl = url;
    public void SetPlayerId(string playerId) => _playerId = playerId;

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
        // Custom JSON object matching backend format
        GameDataWrapper wrapper = new GameDataWrapper
        {
            userId = _playerId,
            character = data.character,
            map = data.map
        };

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

                // Giải mã wrapper -> gameData
                GameDataResponseWrapper response = JsonUtility.FromJson<GameDataResponseWrapper>(json);
                onLoaded?.Invoke((T)response.gameData);
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
    private class GameDataWrapper
    {
        public string userId;
        [FormerlySerializedAs("player")] public CharacterData character;
        public MapData map;
    }

    [System.Serializable]
    private class GameDataResponseWrapper
    {
        public GameData gameData;
    }
}
