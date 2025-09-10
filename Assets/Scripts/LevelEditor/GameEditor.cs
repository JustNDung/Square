using UnityEngine;
using System.Collections.Generic;

public class GameEditor : MonoBehaviour
{
    private MapEditor _mapEditor;
    private LevelEditor _levelEditor;
    private List<TileEditor> _tileEditors;
    private List<CharacterEditor> _characterEditors;
    
    private void Awake()
    {
        _tileEditors = new List<TileEditor>();
        _characterEditors = new List<CharacterEditor>();
    }
    
    // Parse level editor data to game data and save.
    public void SaveLevelEditor()
    {
        List<TileData> tileDatas = new List<TileData>();
        for (int i = 0; i < _tileEditors.Count; i++)
        {
            TileEditorData tileEditorData = _tileEditors[i].GetData();
            TileData tileData = new TileData
            {
                type =  tileEditorData.type,
                posX = tileEditorData.posX,
                posY = tileEditorData.posY,
                posZ = tileEditorData.posZ
            };
            tileDatas.Add(tileData);
        }

        List<CharacterData> characterDatas = new List<CharacterData>();
        for (int i = 0; i < _characterEditors.Count; i++)
        {
            CharacterEditorData characterEditorData = _characterEditors[i].GetData();
            CharacterData characterData = new CharacterData
            {
                type =  characterEditorData.type,
                posX = characterEditorData.initialPosition.x,
                posY = characterEditorData.initialPosition.y,
                posZ = characterEditorData.initialPosition.z,
            };
            characterDatas.Add(characterData);
        }

        GameLevelData gameLevelData = new GameLevelData
        {
            level = new LevelData
            {
                levelNum = _levelEditor.GetData().levelNum,
                chapterNum = _levelEditor.GetData().chapterNum
            },
            map = new MapData
            {
                width = _mapEditor.GetData().width,
                length = _mapEditor.GetData().length
            },

            tiles = tileDatas,
            characters = characterDatas
        };

        SaveLoadService.SaveGameLevel(gameLevelData);
    }
    
    public void DeleteData()
    {
        _mapEditor = null;
        _levelEditor = null;
        _tileEditors.Clear();
        _characterEditors.Clear();
    }

    public void LoadLevelEditor(string chapterNum, string levelNum)
    {
        SaveLoadService.LoadGameLevel(chapterNum, levelNum, (gameLevelData) =>
        {
            if (gameLevelData != null)
            {
                LevelData levelData = gameLevelData.level;
                
                LevelEditorData levelEditorData = new LevelEditorData
                {
                    levelNum = levelData.levelNum,
                    chapterNum = levelData.chapterNum
                };
                LevelManager.Instance.LevelEditor.Apply(levelEditorData);

                MapManager.Instance.GenerateMapFromData(gameLevelData);
            }
        });
    }
    
    // Getters and Setters
    
    public MapEditor MapEditor
    {
        get { return _mapEditor; }
        set { _mapEditor = value; }
    }
    
    public LevelEditor LevelEditor
    {
        get { return _levelEditor; }
        set { _levelEditor = value; }
    }
    
    public List<TileEditor> TileEditors
    {
        get { return _tileEditors; }
        set { _tileEditors = value; }
    }

    public List<CharacterEditor> CharacterEditors
    {
        get { return _characterEditors; }
        set { _characterEditors = value; }
    }

}
        
    
