using UnityEngine;
using System;
using System.Collections.Generic;

public static class SaveLoadService
{
    private static UserSaveLoadManager _userManager;
    private static CharacterSaveLoadManager _characterManager;
    private static MapSaveLoadManager _mapManager;
    private static LevelSaveLoadManager _levelManager;
    private static TileSaveLoadManager _tileManager;

    public static void Initialize()
    {
        _userManager = new UserSaveLoadManager();
        _characterManager = new CharacterSaveLoadManager();
        _mapManager = new MapSaveLoadManager();
        _levelManager = new LevelSaveLoadManager();
        _tileManager = new TileSaveLoadManager();
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

    // ===== CHARACTER =====
    public static void SaveCharacter(CharacterData data, Action<bool> onComplete = null)
    {
        _characterManager.Save(data, onComplete);
    }

    public static void LoadCharacter(string mapId, Action<CharacterData> onLoaded)
    {
        _characterManager.Load(mapId, onLoaded);
    }

    // ===== MAP =====
    public static void SaveMap(MapData data, Action<bool> onComplete = null)
    {
        _mapManager.Save(data, onComplete);
    }

    public static void LoadMap(string levelId, Action<MapData> onLoaded)
    {
        _mapManager.Load(levelId, onLoaded);
    }

    // ===== LEVELS =====
    public static void SaveLevel(LevelData level, Action<bool> onComplete = null)
    {
        _levelManager.Save(level, onComplete);
    }

    public static void LoadLevel(string userId, Action<LevelData> onLoaded)
    {
        _levelManager.Load(userId, onLoaded);
    }

    // ===== TILES =====
    public static void SaveTile(TileData tiles, Action<bool> onComplete = null)
    {
        _tileManager.Save(tiles, onComplete);
    }

    public static void LoadTile(string userId, Action<TileData> onLoaded)
    {
        _tileManager.Load(userId, onLoaded);
    }
}
