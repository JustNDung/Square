using UnityEngine;
using System;

public static class SaveLoadService
{
    private static ISaveLoadManager<GameData> _manager;
    private static string _currentUserId;

    public static void Initialize(MonoBehaviour context, string serverUrl, string userId)
    {
        if (_manager == null)
        {
            var manager = new ServerSaveLoadManager<GameData>();
            manager.SetServerUrl(serverUrl);
            _manager = manager;
        }

        _currentUserId = userId;
    }

    public static void Save(GameData data, Action<bool> onComplete = null)
    {
        if (_manager == null)
        {
            Debug.LogError("SaveLoadService not initialized!");
            onComplete?.Invoke(false);
            return;
        }

        _manager.Save(data, onComplete);
    }

    public static void Load(Action<GameData> onLoaded)
    {
        if (_manager == null)
        {
            Debug.LogError("SaveLoadService not initialized!");
            onLoaded?.Invoke(null);
            return;
        }

        if (string.IsNullOrEmpty(_currentUserId))
        {
            Debug.LogError("No userId set. Call Initialize() with userId first.");
            onLoaded?.Invoke(null);
            return;
        }

        _manager.Load(_currentUserId, onLoaded);
    }

    public static string CurrentUserId => _currentUserId;
}