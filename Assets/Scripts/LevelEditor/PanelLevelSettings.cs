using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelLevelSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelIdIpt;
    [SerializeField] private Button saveLevelBtn;
    [SerializeField] private Button loadLevelBtn;
    [SerializeField] private Button deleteLevelBtn;
    [SerializeField] private int defaultLevelId = 1; // Default level ID
    private void Awake()
    {
        saveLevelBtn.onClick.AddListener(OnSaveLevelEditor);
        levelIdIpt.onEndEdit.AddListener(OnEndEditLevelId);
        loadLevelBtn.onClick.AddListener(OnLoadLevelEditor);
    }
    
    private void Start()
    {
        SetDefaultValues();
    }
    
    private void SetDefaultValues()
    {
        levelIdIpt.text = defaultLevelId.ToString();
        ApplyLevelEditorData(defaultLevelId);
    }
    
    private void OnEndEditLevelId(string value)
    {
        if (int.TryParse(value, out int levelId))
        {
            ApplyLevelEditorData(levelId);
        }
        else
        {
            Debug.LogError("Invalid input for level ID.");
        }
    }

    private void ApplyLevelEditorData(int levelId)
    {
        // Update the level ID in the game manager or relevant system
        LevelEditorData levelEditorData = new LevelEditorData
        {
            levelId = levelId
        };

        LevelManager.Instance.LevelEditor.Apply(levelEditorData);
    }

    private void OnSaveLevelEditor()
    {
        GameManager.Instance.GameEditor.DeleteData(); // Clear previous data
        MessageDispatcher.Send(GameEvent.SaveLevelEditor); // Notify all subscribers to save their data
        GameManager.Instance.GameEditor.SaveLevelEditor(); // Save the level editor data
    }

    private void OnLoadLevelEditor()
    {
        GameManager.Instance.GameEditor.LoadLevelEditor(levelIdIpt.text);
    }
    
}
