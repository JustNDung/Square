using UnityEngine;
public class CharacterEditor : MonoBehaviour, IEditorInteractable, IDataProvider
{
    private CharacterController _characterController;
    private void Awake()
    {       
        _characterController = GetComponent<CharacterController>();
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }

    public void Apply(CharacterEditorData characterEditorData)
    {
        Vector3 newPos = characterEditorData.initialPosition;
        MapManager.Instance.MapState.ModifyCharacterPosition(_characterController, newPos);
    }
    
    public CharacterEditorData GetData()
    {
        return new CharacterEditorData
        {
            initialPosition = _characterController.transform.position
        };
    }
    
    public void OnEditorRightClick()
    {
        MessageDispatcher.Send(GameEvent.OnCharacterEditorRightClick, this);
    }
    
    public void OnESCDown()
    {
        MessageDispatcher.Send(GameEvent.OnCharacterEditorLeftClick);
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.CharacterEditors.Add(this);
    }
    
    // Getters Setters
    public CharacterController CharacterController => _characterController;
    
}

[System.Serializable]
public class CharacterEditorData
{
    public Vector3 initialPosition;
}