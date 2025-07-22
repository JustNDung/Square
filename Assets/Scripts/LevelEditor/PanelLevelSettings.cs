using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelLevelSettings : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private TMP_InputField levelIdIpt;
    
    [Header("Map Settings")]
    [SerializeField] private TMP_InputField mapWidthIpt;
    [SerializeField] private TMP_InputField mapLengthIpt;
    [SerializeField] private Button generateMapBtn;

    [Header("Character Settings")]
    [SerializeField] private Vector3 characterPosition;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Set default values for inputs
        levelIdIpt.text = "1"; // Default level ID
        mapWidthIpt.text = "5"; // Default map width
        mapLengthIpt.text = "5"; // Default map length
        
        // Add listener to the generate map button
        generateMapBtn.onClick.AddListener(OnClickGenerateMapButton);
    }
    
    private void OnClickGenerateMapButton()
    {
        if (int.TryParse(mapWidthIpt.text, out int width) && int.TryParse(mapLengthIpt.text, out int length))
        {
            MapManager.Instance.GenerateMap(width, length);
        }
        else
        {
            Debug.LogError("Invalid map dimensions input.");
        }
    }

}
