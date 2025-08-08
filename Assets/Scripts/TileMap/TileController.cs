using UnityEngine;

[RequireComponent(typeof(TileView))]
public class TileController : MonoBehaviour
{
    private TileModel _model;
    private TileView _view;

    private void Awake()
    {
        _view = GetComponent<TileView>();
        _model = new TileModel(transform.position, TileType.None); // Initialize with default values
    }
    
    public void Apply(TileModel model)
    {
        _model = model;
        _view.UpdateView(model.TileType);
        transform.position = model.TilePos;
        MapManager.Instance.MapState.AddSpecialTile(_model);
    }
    
    #region Getters and Setters
    public TileModel Model
    {
        get => _model;
        set => _model = value;
    }

    public TileView View
    {
        get => _view;
        set => _view = value;
    }
    #endregion
    

    
    
    
    
}
