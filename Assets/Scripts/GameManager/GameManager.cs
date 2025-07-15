using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Server Settings")]
    [SerializeField] private string serverUrl = "http://localhost:3000/api";
    [SerializeField] private string userId = "player123";

    public GameData CurrentSaveData { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Init SaveLoadService
        SaveLoadService.Initialize(this, serverUrl, userId);
    }

    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        
    }

    public void LoadGame()
    {
        SaveLoadService.Load(data =>
        {
            if (data != null)
            {
                CurrentSaveData = data;
                Debug.Log("✅ Game loaded successfully!");
                // TODO: áp dụng dữ liệu vào game (spawn player, map, v.v.)
            }
            else
            {
                Debug.LogWarning("⚠️ No save data found. Starting new game...");
                CurrentSaveData = CreateDefaultData();
                SaveGame();
            }
            MapManager.Instance.MapInitialize(CurrentSaveData.map);
        });
    }

    public void SaveGame()
    {
        if (CurrentSaveData == null)
        {
            Debug.LogWarning("⚠️ No save data to save.");
            return;
        }

        SaveLoadService.Save(CurrentSaveData, success =>
        {
            if (success)
                Debug.Log("✅ Game saved successfully!");
            else
                Debug.LogError("❌ Failed to save game.");
        });
    }

    private GameData CreateDefaultData()
    {
        return new GameData
        {
            userId = userId,
            player = new PlayerData
            {
                userId = userId,
                level = 1,
                coins = 0,
                position = Vector3.zero
            },
            map = new MapData()
            {
                width = 5,
                length = 5,
            }
        };
    }
}
