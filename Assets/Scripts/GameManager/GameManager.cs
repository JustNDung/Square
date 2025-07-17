using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
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
        SaveLoadService.Initialize();
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
        SaveLoadService.LoadMap("1", map =>
        {
            if (map != null)
            {
                Debug.Log("✅ Map loaded successfully.");
                MapManager.Instance.SetupMap(map);
            }
            else
            {
                Debug.LogWarning("⚠️ Failed to load map.");
            }
        });
        
        SaveLoadService.LoadCharacter("1", character =>
        {
            if (character != null)
            {
                Debug.Log("✅ Character loaded successfully.");
                MapManager.Instance.SetupCharacters(character);
            }
            else
            {
                Debug.LogWarning("⚠️ Failed to load character.");
            }
        });
    }

    public void SaveGame()
    {
        
    }

    
}
