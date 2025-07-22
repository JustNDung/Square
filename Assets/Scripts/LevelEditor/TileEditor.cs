
using UnityEngine;

public class TileEditor : MonoBehaviour, IEditorInteractable
{
    private Tile _tile;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float obstacleY = 1f;
    private GameObject _obstacle;
    
    private void Awake()
    {
        _tile = GetComponent<Tile>();
    }
    
    public void OnEditorRightClick()        
    {
        MessageDispatcher.Send(GameEvent.OnTileEditorRightClick, this);
    }
    
    public void Apply(TileEditorData tileEditorData)
    {
        _tile.IsWalkable = tileEditorData.isWalkable;
        SetupObstacle(tileEditorData);
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
}


[System.Serializable]
public class TileEditorData
{
    public bool isWalkable;
}
