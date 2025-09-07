using UnityEngine;


public class LevelEditor : MonoBehaviour, IDataProvider
{
    private void Awake()
    {
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }

    public void Apply(LevelEditorData levelEditorData)
    {
        LevelManager.Instance.CurrentLevelId = levelEditorData.levelId;
        LevelManager.Instance.CurrentChapterId = levelEditorData.chapterId;
    }

    public LevelEditorData GetData()
    {
        return new LevelEditorData
        {
            levelId = LevelManager.Instance.CurrentLevelId,
            chapterId = LevelManager.Instance.CurrentChapterId
        };
    }
    
    private void OnSaveLevelEditor(object args)
    {
        GameManager.Instance.GameEditor.LevelEditor = this;
    }
    
    private void OnDestroy()
    {
        MessageDispatcher.Unsubscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }
}

[System.Serializable]
public class LevelEditorData
{
    public int levelId;
    public int chapterId;
}
