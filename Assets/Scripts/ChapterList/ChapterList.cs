using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterList : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private int leverTextFontSize = 24;

    private Button _currentSelected;
    private List<Button> _buttons = new();

    private readonly int[,] _chapters = new int[,]
    {
        { 1, 9 },
        { 2, 12 },
        { 3, 12 },
        { 4, 18 },
        { 5, 24 },
        { 6, 12 }
    };

    private void Start()
    {
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        for (int chapter = 0; chapter < content.childCount; chapter++)
        {
            Transform panel = content.GetChild(chapter).GetChild(0).GetChild(0);
            int levelCount = _chapters[chapter, 1];

            for (int i = 0; i < levelCount; i++)
            {
                int currentLevel = i + 1; // reset mỗi chapter

                GameObject buttonObj = Instantiate(levelButtonPrefab, panel);
                buttonObj.name = $"Chapter_{chapter + 1}_Level_{currentLevel}";

                Text btnText = buttonObj.GetComponentInChildren<Text>();
                if (btnText != null)
                {
                    btnText.text = currentLevel.ToString();
                    btnText.fontSize = leverTextFontSize;
                }

                Button btn = buttonObj.GetComponent<Button>();

                SetupButtonColors(btn);

                int chapterIndex = chapter + 1;

                btn.onClick.AddListener(() =>
                {
                    SelectButton(btn);
                    OnLevelSelected(chapterIndex, currentLevel);
                });

                _buttons.Add(btn);
            }
        }
    }

    private void SetupButtonColors(Button btn)
    {
        ColorBlock colors = btn.colors;

        colors.normalColor = Color.white;   
        colors.highlightedColor = new Color(0.85f, 0.9f, 1f); // hover
        colors.pressedColor = new Color(0.6f, 0.7f, 1f);      // click
        colors.selectedColor = Color.yellow;                  // level đã chọn
        colors.disabledColor = Color.gray;

        colors.fadeDuration = 0.1f;

        btn.colors = colors;    
    }

    private void SelectButton(Button btn)
    {
        if (_currentSelected != null)
            _currentSelected.OnDeselect(null);

        btn.Select();
        _currentSelected = btn;
    }

    private void OnLevelSelected(int chapter, int level)
    {
        Debug.Log($"Selected Chapter {chapter} - Level {level}");
    }
}