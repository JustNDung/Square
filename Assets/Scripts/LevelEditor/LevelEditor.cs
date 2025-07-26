using UnityEngine;


public class LevelEditor : MonoBehaviour, IDataProvider
{
    private void Awake()
    {
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
    
    public void Apply (LevelEditorData levelEditorData)
    {
        LevelManager.Instance.CurrentLevelId = levelEditorData.levelId;
    }

    public LevelEditorData GetData()
    {
        return new LevelEditorData
        {
            levelId = LevelManager.Instance.CurrentLevelId
        };
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.LevelEditor = this;
    }
}

[System.Serializable]
public class LevelEditorData
{
    public int levelId;
}
