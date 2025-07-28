using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PanelCharacterModify : MonoBehaviour
{
    [SerializeField] private TMP_InputField characterCurrentX;
    [SerializeField] private TMP_InputField characterCurrentY;
    [SerializeField] private TMP_InputField characterCurrentZ;
    
    [SerializeField] private TMP_InputField characterInitialX;
    [SerializeField] private TMP_InputField characterInitialY;
    [SerializeField] private TMP_InputField characterInitialZ;
    
    [SerializeField] private Button deleteCharacterButton;

    private CharacterEditor _characterEditor;
    private CharacterEditorData _characterEditorData;
    private void Awake()
    {
        gameObject.SetActive(false);
        
        characterCurrentY.interactable = false; // Disable Y input as it's usually constant for characters
        characterCurrentX.interactable = false;
        characterCurrentZ.interactable = false;
        
        characterInitialY.interactable = false; // Disable Y input as it's usually constant for characters
        characterInitialX.interactable = false;
        characterInitialZ.interactable = false;
        
        deleteCharacterButton.onClick.AddListener(DeleteCharacter);
        
        MessageDispatcher.Subscribe(GameEvent.OnCharacterEditorRightClick, OnCharacterEditorRightClick);
        MessageDispatcher.Subscribe(GameEvent.OnCharacterEditorLeftClick, OnCharacterEditorLeftClick);
    }

    private void Update()
    {
        DisplayCurrentCharacterPosition();
    }
    
    private void DisplayCurrentCharacterPosition()
    {
        if (_characterEditor != null)
        {
            // Update the current position input fields with the character's current position
            Vector3 currentPosition = _characterEditor.CharacterController.transform.position;
            characterCurrentX.text = currentPosition.x.ToString("F2");
            characterCurrentY.text = currentPosition.y.ToString("F2");
            characterCurrentZ.text = currentPosition.z.ToString("F2");
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
            characterCurrentX.text = _characterEditorData.currentPosition.x.ToString("F2");
            characterCurrentY.text = _characterEditorData.currentPosition.y.ToString("F2");
            characterCurrentZ.text = _characterEditorData.currentPosition.z.ToString("F2");
            
            characterInitialX.text = _characterEditorData.initialPosition.x.ToString("F2");
            characterInitialY.text = _characterEditorData.initialPosition.y.ToString("F2");
            characterInitialZ.text = _characterEditorData.initialPosition.z.ToString("F2");
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
            if (_characterEditor.CharacterController != null)
            {
                // Remove the character from the map state
                MapManager.Instance.MapState.RemoveCharacter(_characterEditor.CharacterController);
                // Destroy the character GameObject
                Destroy(_characterEditor.CharacterController.gameObject);
                Destroy(_characterEditor.CharacterController.CharacterBodyContainer);
            }
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        // MessageDispatcher.Unsubscribe(GameEvent.OnCharacterEditorRightClick, OnCharacterEditorRightClick);
        // MessageDispatcher.Unsubscribe(GameEvent.OnCharacterEditorLeftClick, OnCharacterEditorLeftClick);
    }
}
