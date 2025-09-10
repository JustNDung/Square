using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelLevelSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelNumIpt;
    [SerializeField] private TMP_InputField chapterNumIpt;
    [SerializeField] private Button saveLevelBtn;
    [SerializeField] private Button loadLevelBtn;
    [SerializeField] private Button deleteLevelBtn;
    [SerializeField] private int defaultLevelId = 1; // Default level ID
    [SerializeField] private int defaultChapterId = 1; // Default chapter ID
    private int currentLevelNum;
    private int currentChapterNum;
    
    private void Awake()
    {
        saveLevelBtn.onClick.AddListener(OnSaveLevelEditor);
        levelNumIpt.onEndEdit.AddListener(OnEndEditLevelId);
        chapterNumIpt.onEndEdit.AddListener(OnEndEditChapterId);
        loadLevelBtn.onClick.AddListener(OnLoadLevelEditor);
    }
    
    private void Start()
    {
        SetDefaultValues();
    }
    
    private void SetDefaultValues()
    {
        currentChapterNum = defaultChapterId;
        currentLevelNum = defaultLevelId;

        levelNumIpt.text = defaultLevelId.ToString();
        chapterNumIpt.text = defaultChapterId.ToString();
        ApplyLevelEditorData(currentLevelNum, currentChapterNum);
    }
    
    private void OnEndEditLevelId(string value)
    {
        if (int.TryParse(value, out int levelId))
        {
            currentLevelNum = levelId;
            ApplyLevelEditorData(currentLevelNum, currentChapterNum);
        }
        else
        {
            Debug.LogError("Invalid input for level ID.");
        }
    }

    private void OnEndEditChapterId(string value)
    {
        if (int.TryParse(value, out int chapterId))
        {
            currentChapterNum = chapterId;
            ApplyLevelEditorData(currentLevelNum, currentChapterNum);
        }
        else
        {
            Debug.LogError("Invalid input for chapter ID.");
        }
    }

    
    private void ApplyLevelEditorData(int levelId, int chapterId)
    {
        // Update the level ID in the game manager or relevant system
        LevelEditorData levelEditorData = new LevelEditorData
        {
            levelNum = levelId,
            chapterNum = chapterId
        };

        LevelManager.Instance.LevelEditor.Apply(levelEditorData);
    }

    private void OnSaveLevelEditor()
    {
        ApplyLevelEditorData(currentLevelNum, currentChapterNum);
        GameManager.Instance.GameEditor.DeleteData(); // Clear previous data
        MessageDispatcher.Send(GameEvent.SaveLevelEditor); // Notify all subscribers to save their data
        GameManager.Instance.GameEditor.SaveLevelEditor(); // Save the level editor data
    }

    private void OnLoadLevelEditor()
    {
        GameManager.Instance.GameEditor.LoadLevelEditor(chapterNumIpt.text, levelNumIpt.text);
    }
    
}
