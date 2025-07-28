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

    public void SaveLevelEditor()
    {
        List<TileData> tileDatas = new List<TileData>();
        for (int i = 0; i < _tileEditors.Count; i++)
        {
            TileEditorData tileEditorData = _tileEditors[i].GetData();
            TileData tileData = new TileData
            {
                levelId = _levelEditor.GetData().levelId,
                isWalkable = tileEditorData.isWalkable
            };
            tileDatas.Add(tileData);
        }

        List<CharacterData> characterDatas = new List<CharacterData>();
        for (int i = 0; i < _characterEditors.Count; i++)
        {
            CharacterEditorData characterEditorData = _characterEditors[i].GetData();
            CharacterData characterData = new CharacterData
            {
                levelId = _levelEditor.GetData().levelId,
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
                levelId = _levelEditor.GetData().levelId
            },
            map = new MapData
            {
                levelId = _levelEditor.GetData().levelId,
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
        
    
