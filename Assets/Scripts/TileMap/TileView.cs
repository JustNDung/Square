using UnityEngine;

public class TileView : MonoBehaviour
{ 
    private Transform _effectsContainer;
    [SerializeField] private GameObject obstacle;
    
    [SerializeField] private GameObject teleport;
    
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject up;

    [SerializeField] private GameObject horizontal;
    [SerializeField] private GameObject vertical;

    public void UpdateView(TileType tileType)
    {
        CreateEffectsContainer();
        switch (tileType)
        {
            case TileType.Obstacle:
                CreateObstacle();
                break;
            case TileType.Teleport:
                CreateTeleport();
                break;
            case TileType.Down:
                CreateDown();
                break;
            case TileType.Left:
                CreateLeft();
                break;
            case TileType.Right:
                CreateRight();
                break;
            case TileType.Up:
                CreateUp();
                break;
            case TileType.Horizontal:
                CreateHorizontal();
                break;
            case TileType.Vertical:
                CreateVertical();
                break;
            default:
                if (_effectsContainer != null)
                {
                    Destroy(_effectsContainer.gameObject);
                }
                break;
        }
    }
    
    private void CreateDown()
    {
        GameObject d = Instantiate(down, _effectsContainer);
        d.transform.localPosition = new Vector3(0, 0.15f, 0);
    }

    private void CreateLeft()
    {
        GameObject l = Instantiate(left, _effectsContainer);
        left.transform.localPosition = new Vector3(0, 0.15f, 0);
    }

    private void CreateRight()
    {
        GameObject r = Instantiate(right, _effectsContainer);
        r.transform.localPosition = new Vector3(0, 0.15f, 0);
    }

    private void CreateUp()
    {
        GameObject u = Instantiate(up, _effectsContainer);
        u.transform.localPosition = new Vector3(0, 0.15f, 0);
    }
    
    private void CreateObstacle()
    {
        GameObject obs = Instantiate(obstacle, _effectsContainer);
        obs.transform.localPosition = new Vector3(0, 1f, 0);
    }
    
    private void CreateTeleport()
    {
        GameObject tele = Instantiate(teleport, _effectsContainer);
        tele.transform.localPosition = new Vector3(0, 1f, 0);
    }

    private void CreateHorizontal()
    {
        
    }

    private void CreateVertical()
    {
        
    }

    private void CreateEffectsContainer()
    {
        
        if (_effectsContainer != null)
        {
            Destroy(_effectsContainer.gameObject);
            _effectsContainer = null;
        }
        
        _effectsContainer = new GameObject("EffectsContainer").transform;
        _effectsContainer.SetParent(transform);
        _effectsContainer.localPosition = Vector3.zero;
    }

}
