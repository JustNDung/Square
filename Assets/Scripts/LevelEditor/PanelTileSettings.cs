
using UnityEngine;
using UnityEngine.UI;

public class PanelTileSettings : MonoBehaviour
{
    private TileEditor _targetTileEditor;
    private TileEditorData _currentTileData;

    [Header("UI Elements")]
    [SerializeField] private Toggle walkableToggle;
    private void Awake()
    {
        gameObject.SetActive(false);
        
        walkableToggle.onValueChanged.AddListener(OnWalkableToggleChanged);
        
        MessageDispatcher.Subscribe(GameEvent.OnTileEditorRightClick, OnTileEditorRightClick);
        MessageDispatcher.Subscribe(GameEvent.OnTileEditorLeftClick, OnTileEditorLeftClick);
        
    }

    private void OnWalkableToggleChanged(bool isWalkable)
    {
        if (_targetTileEditor == null || _currentTileData == null) return;

        _currentTileData.isWalkable = isWalkable;
        _targetTileEditor.Apply(_currentTileData);

        UpdateTileState(isWalkable);
    }

    private void UpdateTileState(bool isWalkable)
    {
        if (!isWalkable)
        {
            MapManager.Instance.MapState.AddUnwalkableTile(_targetTileEditor.transform.position);
        }
        else
        {
            MapManager.Instance.MapState.RemoveUnwalkableTile(_targetTileEditor.transform.position);
        }
    }

    private void OnTileEditorRightClick(object args)
    {
        gameObject.SetActive(true);
        if (args is TileEditor targetTileEditor)
        {
            _targetTileEditor = targetTileEditor;
            _currentTileData = _targetTileEditor.GetData();

            walkableToggle.isOn = _currentTileData.isWalkable;
        }
    }
    
    private void OnTileEditorLeftClick(object args)
    {
        // Handle left click if needed
        // This could be used to close the panel or perform other actions
        gameObject.SetActive(false);
    }
    

    private void OnDisable()
    {
        // MessageDispatcher.Unsubscribe(GameEvent.OnTileEditorRightClick, OnTileEditorRightClick);
        // MessageDispatcher.Unsubscribe(GameEvent.OnTileEditorLeftClick, OnTileEditorLeftClick);
    }
}