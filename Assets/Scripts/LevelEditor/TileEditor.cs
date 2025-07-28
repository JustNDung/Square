
using UnityEngine;

public class TileEditor : MonoBehaviour, IEditorInteractable, IDataProvider
{
    private Tile _tile;
    private GameObject _obstacle;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float obstacleY = 1f;
    
    
    private void Awake()
    {
        _tile = GetComponent<Tile>();
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

    public void Apply(TileEditorData tileEditorData)
    {
        _tile.IsWalkable = tileEditorData.isWalkable;
        SetupObstacle(tileEditorData);
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.TileEditors.Add(this);
    }

    private void SetupObstacle(TileEditorData tileEditorData)
    {
        if (!tileEditorData.isWalkable)
        {
            GenerateObstacle();
        }
        else
        {
            Destroy(_obstacle);
        }
    }

    public TileEditorData GetData()
    {
        return new TileEditorData
        {
            isWalkable = _tile.IsWalkable
        };
    }

    private void GenerateObstacle()
    {
        Transform obstacleContainer = MapManager.Instance.transform.Find("ObstacleContainer");
        _obstacle = Instantiate(obstaclePrefab, new Vector3(transform.position.x, obstacleY, transform.position.z), 
            Quaternion.identity, obstacleContainer);
    }

    private void OnDestroy()
    {
        MessageDispatcher.Unsubscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
}


[System.Serializable]
public class TileEditorData
{
    public bool isWalkable;
}
