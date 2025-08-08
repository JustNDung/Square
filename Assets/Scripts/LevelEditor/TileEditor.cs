
using UnityEngine;

public class TileEditor : MonoBehaviour, IEditorInteractable, IDataProvider
{
    private TileController _tileController;
    
    private void Awake()
    {
        _tileController = GetComponent<TileController>();
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
    
    public void OnEditorRightClick()        
    {
        MessageDispatcher.Send(GameEvent.OnTileEditorRightClick, this);
    }       

    public void OnESCDown()
    {
        MessageDispatcher.Send(GameEvent.OnTileEditorLeftClick);
    }
    
    public void ClosePopUp()
    {
        MessageDispatcher.Send(GameEvent.ClosePopUp);
    }

    public void Apply(TileEditorData tileEditorData)
    {
        TileModel tileModel = new TileModel(new Vector3(tileEditorData.posX, tileEditorData.posY, tileEditorData.posZ), 
            tileEditorData.tileType);
        _tileController.Apply(tileModel);
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.TileEditors.Add(this);
    }

    public TileEditorData GetData()
    {
        return new TileEditorData
        {
            tileType = _tileController.Model.TileType,
            posX = _tileController.transform.position.x,
            posY = _tileController.transform.position.y,
            posZ = _tileController.transform.position.z
        };
    }

    private void OnDestroy()
    {
        MessageDispatcher.Unsubscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
}


[System.Serializable]
public class TileEditorData
{
    public TileType tileType;
    public float posX;
    public float posY;
    public float posZ;
}
