using UnityEngine;
using System;
using System.Collections.Generic;

public static class SaveLoadService
{
    private static UserSaveLoadManager _userManager;
    private static GameLevelSaveLoadManager _gameLevelManager;


    public static void Initialize()
    {
        _userManager = new UserSaveLoadManager();
        _gameLevelManager = new GameLevelSaveLoadManager();
    }

    // ===== USER =====
    public static void SaveUser(UserData data, Action<bool> onComplete = null)
    {
        _userManager.Save(data, onComplete);
    }

    public static void LoadUser(string userId, Action<UserData> onLoaded)
    {
        _userManager.Load(userId, onLoaded);
    }

    // ===== GAME LEVEL =====
    public static void SaveGameLevel(GameLevelData data, Action<bool> onComplete = null)
    {
        _gameLevelManager.Save(data, onComplete);
    }

    public static void LoadGameLevel(string levelId, Action<GameLevelData> onLoaded)
    {
        _gameLevelManager.Load(levelId, onLoaded);
    }

    
}
