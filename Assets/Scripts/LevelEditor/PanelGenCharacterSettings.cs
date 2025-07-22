
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PanelGenCharacterSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField characterX;
    [SerializeField] private TMP_InputField characterY;
    [SerializeField] private TMP_InputField characterZ;
    [SerializeField] private Button generateCharacterButton;

    private void Awake()
    {
        SetDefaultValues();
        generateCharacterButton.onClick.AddListener(OnClickGenerateCharacterButton);
    }
    
    private void OnClickGenerateCharacterButton()
    {
        if (float.TryParse(characterX.text, out float x) &&
            float.TryParse(characterY.text, out float y) &&
            float.TryParse(characterZ.text, out float z))
        {
            MapManager.Instance.GenerateCharacter(new Vector3(x, y, z));
        }
        else
        {
            Debug.LogError("Invalid input for character position.");
        }
    }
    
    private void SetDefaultValues()
    {
        characterX.text = "0";
        characterY.text = "0.25";
        characterZ.text = "0";
    }
    
}
