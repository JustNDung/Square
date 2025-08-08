
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelTileSettings : MonoBehaviour
{
    private TileEditor _targetTileEditor;
    private TileEditorData _currentTileData;

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown tileTypeDropdown;
    [SerializeField] private TMP_InputField tilePosX;
    [SerializeField] private TMP_InputField tilePosY;
    [SerializeField] private TMP_InputField tilePosZ;
    private void Awake()
    {
        gameObject.SetActive(false);
        
        tilePosX.interactable = false;
        tilePosY.interactable = false;
        tilePosZ.interactable = false;
        
        InitTileTypeDropdown();
        
        MessageDispatcher.Subscribe(GameEvent.OnTileEditorRightClick, OnTileEditorRightClick);
        MessageDispatcher.Subscribe(GameEvent.OnTileEditorLeftClick, OnTileEditorLeftClick);
        MessageDispatcher.Subscribe(GameEvent.ClosePopUp, ClosePopUp);
    }
    
    private void InitTileTypeDropdown()
    {
        tileTypeDropdown.ClearOptions();

        var tileTypes = Enum.GetNames(typeof(TileType));
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var typeName in tileTypes)
        {
            options.Add(new TMP_Dropdown.OptionData(typeName));
        }

        tileTypeDropdown.AddOptions(options);
        tileTypeDropdown.onValueChanged.AddListener(OnTileTypeChanged);
    }
    
    private void OnTileTypeChanged(int index)
    {
        if (_targetTileEditor == null || _currentTileData == null) return;

        _currentTileData.tileType = (TileType)index;
        _targetTileEditor.Apply(_currentTileData);
    }

    private void OnTileEditorRightClick(object args)
    {
        gameObject.SetActive(true);
        if (args is TileEditor targetTileEditor)
        {
            _targetTileEditor = targetTileEditor;
            _currentTileData = _targetTileEditor.GetData();
            
            tileTypeDropdown.value = (int)_currentTileData.tileType;
            tilePosX.text = _currentTileData.posX.ToString("F2");
            tilePosY.text = _currentTileData.posY.ToString("F2");
            tilePosZ.text = _currentTileData.posZ.ToString("F2");
        }
    }
    
    private void OnTileEditorLeftClick(object args)
    {
        gameObject.SetActive(false);
    }
    
    private void ClosePopUp(object args)
    {
        gameObject.SetActive(false);
    }
    

    private void OnDisable()
    {
        // MessageDispatcher.Unsubscribe(GameEvent.OnTileEditorRightClick, OnTileEditorRightClick);
        // MessageDispatcher.Unsubscribe(GameEvent.OnTileEditorLeftClick, OnTileEditorLeftClick);
    }
}