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
        
    }

    public void SaveGame()
    {
        
    }

    
}
