using UnityEngine;


public class MapEditor : MonoBehaviour
{
    private void Awake()
    {
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
    public void Apply(MapEditorData mapEditorData)
    {
        MapManager.Instance.MapWidth = mapEditorData.width;
        MapManager.Instance.MapLength = mapEditorData.length;
    }
    
    public MapEditorData GetData()
    {
        return new MapEditorData
        {
            width = MapManager.Instance.MapWidth,
            length = MapManager.Instance.MapLength
        };
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.MapEditor = this;
    }
    
}

[System.Serializable]
public class MapEditorData
{
    public int width;
    public int length;
}