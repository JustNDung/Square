using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PanelCharacterModify : MonoBehaviour
{
    [SerializeField] private TMP_InputField characterX;
    [SerializeField] private TMP_InputField characterY;
    [SerializeField] private TMP_InputField characterZ;
    [SerializeField] private Button deleteCharacterButton;

    private CharacterEditor _characterEditor;
    private CharacterEditorData _characterEditorData;
    private void Awake()
    {
        gameObject.SetActive(false);
        
        characterY.interactable = false; // Disable Y input as it's usually constant for characters
        
        characterX.onEndEdit.AddListener(OnEndEditCharacterX);
        characterY.onEndEdit.AddListener(OnEndEditCharacterY);
        characterZ.onEndEdit.AddListener(OnEndEditCharacterZ);
        deleteCharacterButton.onClick.AddListener(DeleteCharacter);
        
        MessageDispatcher.Subscribe(GameEvent.OnCharacterEditorRightClick, OnCharacterEditorRightClick);
        MessageDispatcher.Subscribe(GameEvent.OnCharacterEditorLeftClick, OnCharacterEditorLeftClick);
    }
    
    private void OnEndEditCharacterX(string value)
    {
        if (float.TryParse(value, out float x))
        {
            _characterEditorData.initialPosition.x = x;
            _characterEditor.Apply(_characterEditorData);
        }
        else
        {
            Debug.LogError("Invalid input for character X position.");
        }
    }
    
    private void OnEndEditCharacterY(string value)
    {
        if (float.TryParse(value, out float y))
        {
            _characterEditorData.initialPosition.y = y;
            _characterEditor.Apply(_characterEditorData);
        }
        else
        {
            Debug.LogError("Invalid input for character Y position.");
        }
    }

    private void OnEndEditCharacterZ(string value)
    {
        if (float.TryParse(value, out float z))
        {
            _characterEditorData.initialPosition.z = z;
            _characterEditor.Apply(_characterEditorData);
        }
        else
        {
            Debug.LogError("Invalid input for character Z position.");
        }
    }

    private void OnCharacterEditorRightClick(object args)
    {
        if (args is CharacterEditor characterEditor)
        {
            _characterEditor = characterEditor;
            _characterEditorData = _characterEditor.GetData();

            gameObject.SetActive(true);

            // Set input fields with current character position
            characterX.text = _characterEditorData.initialPosition.x.ToString();
            characterY.text = _characterEditorData.initialPosition.y.ToString();
            characterZ.text = _characterEditorData.initialPosition.z.ToString();
            
            // _characterEditor.Apply(_characterEditorData);
        }
    }

    private void OnCharacterEditorLeftClick(object args)
    {
        gameObject.SetActive(false);
    }
    
    private void DeleteCharacter()
    {
        if (_characterEditor != null)
        {
            // Remove character from the map state
            MapManager.Instance.MapState.RemoveCharacter(_characterEditor.CharacterController);
            Destroy(_characterEditor.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // MessageDispatcher.Unsubscribe(GameEvent.OnCharacterEditorRightClick, OnCharacterEditorRightClick);
        // MessageDispatcher.Unsubscribe(GameEvent.OnCharacterEditorLeftClick, OnCharacterEditorLeftClick);
    }
}
