using UnityEngine;


public class LevelEditor : MonoBehaviour, IDataProvider
{
    private void Awake()
    {
        MessageDispatcher.Subscribe(GameEvent.SaveLevelEditor, OnSaveLevelEditor);
    }

    public void Apply(LevelEditorData levelEditorData)
    {
        LevelManager.Instance.CurrentLevelId = levelEditorData.levelNum;
        LevelManager.Instance.CurrentChapterId = levelEditorData.chapterNum;
    }

    public LevelEditorData GetData()
    {
        return new LevelEditorData
        {
            levelNum = LevelManager.Instance.CurrentLevelId,
            chapterNum = LevelManager.Instance.CurrentChapterId
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
    public int levelNum;
    public int chapterNum;
}
