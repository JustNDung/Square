using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameEditor _gameEditor;
    
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
        _gameEditor = GetComponent<GameEditor>();   
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
    
    // Getters and Setters
    public GameEditor GameEditor
    {
        get { return _gameEditor; }
        set { _gameEditor = value; }
    }

    
}
