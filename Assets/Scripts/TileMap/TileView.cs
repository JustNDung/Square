using UnityEngine;

public class TileView : MonoBehaviour
{ 
    private Transform _effectsContainer;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject teleport;

    public void UpdateView(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Obstacle:
                CreateObstacle();
                break;
            case TileType.Teleport:
                CreateTeleport();
                break;
            default:
                if (_effectsContainer != null)
                {
                    Destroy(_effectsContainer.gameObject);
                }
                break;
        }
    }
    
    private void CreateObstacle()
    {
        if (_effectsContainer != null)
        {
            Destroy(_effectsContainer.gameObject);
            _effectsContainer = null;
        }
        
        CreateEffectsContainer();
        
        GameObject obs = Instantiate(obstacle, _effectsContainer);
        obs.transform.localPosition = new Vector3(0, 1f, 0);
    }
    
    private void CreateTeleport()
    {
        if (_effectsContainer != null)
        {
            Destroy(_effectsContainer.gameObject);
            _effectsContainer = null;
        }
        
        CreateEffectsContainer();
        
        GameObject tele = Instantiate(teleport, _effectsContainer);
        tele.transform.localPosition = new Vector3(0, 1f, 0);
    }

    private void CreateEffectsContainer()
    {
        _effectsContainer = new GameObject("EffectsContainer").transform;
        _effectsContainer.SetParent(transform);
        _effectsContainer.localPosition = Vector3.zero;
    }

}
