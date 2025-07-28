// GameModeManager.cs
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    public GameMode CurrentMode = GameMode.LevelEditor;

    private void Awake()
    {
        Instance = this;
    }
}