using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private List<LevelData> allLevels; // Có thể load từ JSON, ScriptableObject, v.v.
    private int _currentLevelId;
    private LevelEditor _levelEditor;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _levelEditor = GetComponent<LevelEditor>();
    }
    
    // Getters and Setters
    public int CurrentLevelId
    {
        get { return _currentLevelId; }
        set { _currentLevelId = value; }
    }
    
    public LevelEditor LevelEditor
    {
        get { return _levelEditor; }
    }
    
}