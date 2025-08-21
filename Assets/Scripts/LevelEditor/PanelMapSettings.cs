using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelMapSettings : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] private TMP_InputField mapWidthIpt;
    [SerializeField] private TMP_InputField mapLengthIpt;
    [SerializeField] private Button generateMapBtn;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {   
        // Set default values for inputs
        mapWidthIpt.text = "5"; // Default map width
        mapLengthIpt.text = "5"; // Default map length
        
        // Add listener to the generate map button
        generateMapBtn.onClick.AddListener(OnClickGenerateMapButton);
    }
    
    private void OnClickGenerateMapButton()
    {
        if (int.TryParse(mapWidthIpt.text, out int width) && int.TryParse(mapLengthIpt.text, out int length))
        {
            MapEditorData mapEditorData = new MapEditorData
            {
                width = Mathf.Max(1, width), // Ensure width is at least 1
                length = Mathf.Max(1, length) // Ensure length is at least 1
            };
            
            MapManager.Instance.MapEditor.Apply(mapEditorData);
            MapManager.Instance.GenerateBasicMapForEditor();
        }
        else
        {
            Debug.LogError("Invalid map dimensions input.");
        }
    }

}
