
using UnityEngine;

public class TileEditor : MonoBehaviour, IEditorInteractable
{
    private Tile tile;
    
    private void Awake()
    {
        tile = GetComponent<Tile>();
    }
    
    public void OnEditorRightClick()
    {
        MessageDispatcher.Send(GameEvent.OnTileEditorRightClick, this);
    }
    
    public void Apply(TileEditorData data)
    {
        tile.IsWalkable = data.isWalkable;
    }

    public TileEditorData GetData()
    {
        return new TileEditorData
        {
            isWalkable = tile.IsWalkable
        };
    }
}


[System.Serializable]
public class TileEditorData
{
    public bool isWalkable;
}
