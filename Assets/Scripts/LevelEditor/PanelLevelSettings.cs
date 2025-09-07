using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelLevelSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelIdIpt;
    [SerializeField] private TMP_InputField chapterIdIpt;
    [SerializeField] private Button saveLevelBtn;
    [SerializeField] private Button loadLevelBtn;
    [SerializeField] private Button deleteLevelBtn;
    [SerializeField] private int defaultLevelId = 1; // Default level ID
    [SerializeField] private int defaultChapterId = 1; // Default chapter ID
    private int currentLevelId;
    private int currentChapterId;
    
    private void Awake()
    {
        saveLevelBtn.onClick.AddListener(OnSaveLevelEditor);
        levelIdIpt.onEndEdit.AddListener(OnEndEditLevelId);
        chapterIdIpt.onEndEdit.AddListener(OnEndEditChapterId);
        loadLevelBtn.onClick.AddListener(OnLoadLevelEditor);
    }
    
    private void Start()
    {
        SetDefaultValues();
    }
    
    private void SetDefaultValues()
    {
        currentChapterId = defaultChapterId;
        currentLevelId = defaultLevelId;

        levelIdIpt.text = defaultLevelId.ToString();
        chapterIdIpt.text = defaultChapterId.ToString();
        ApplyLevelEditorData(currentLevelId, currentChapterId);
    }
    
    private void OnEndEditLevelId(string value)
    {
        if (int.TryParse(value, out int levelId))
        {
            currentLevelId = levelId;
            ApplyLevelEditorData(currentLevelId, currentChapterId);
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
            currentChapterId = chapterId;
            ApplyLevelEditorData(currentLevelId, currentChapterId);
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
            levelId = levelId,
            chapterId = chapterId
        };

        LevelManager.Instance.LevelEditor.Apply(levelEditorData);
    }

    private void OnSaveLevelEditor()
    {
        ApplyLevelEditorData(currentLevelId, currentChapterId);
        GameManager.Instance.GameEditor.DeleteData(); // Clear previous data
        MessageDispatcher.Send(GameEvent.SaveLevelEditor); // Notify all subscribers to save their data
        GameManager.Instance.GameEditor.SaveLevelEditor(); // Save the level editor data
    }

    private void OnLoadLevelEditor()
    {
        GameManager.Instance.GameEditor.LoadLevelEditor(levelIdIpt.text);
    }
    
}
