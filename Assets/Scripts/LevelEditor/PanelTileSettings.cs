
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelTileSettings : MonoBehaviour
{
    private TileEditor _targetTileEditor;
    private TileEditorData _currentTileData;

    [Header("UI Elements")]
    [SerializeField] private Toggle walkableToggle;
    [SerializeField] private TMP_InputField tilePosX;
    [SerializeField] private TMP_InputField tilePosY;
    [SerializeField] private TMP_InputField tilePosZ;
    private void Awake()
    {
        gameObject.SetActive(false);
        
        tilePosX.interactable = false;
        tilePosY.interactable = false;
        tilePosZ.interactable = false;
        
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
            tilePosX.text = _currentTileData.posX.ToString("F2");
            tilePosY.text = _currentTileData.posY.ToString("F2");
            tilePosZ.text = _currentTileData.posZ.ToString("F2");
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