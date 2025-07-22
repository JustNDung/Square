using UnityEngine;
public class CharacterEditor : MonoBehaviour, IEditorInteractable
{
    private CharacterController _characterController;
    private void Awake()
    {       
        _characterController = GetComponent<CharacterController>();
    }

    public void Apply(CharacterEditorData characterEditorData)
    {
        _characterController.transform.position = characterEditorData.initialPosition;
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
    
}

[System.Serializable]
public class CharacterEditorData
{
    public Vector3 initialPosition;
}