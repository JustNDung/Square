using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterList : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject levelButtonPrefab; // Prefab nút level

    private int[,] _chapters = new int[,]
    {
        { 1, 9 },   // Chapter 1 có 9 level
        { 2, 12 },  // Chapter 2 có 12 level
        { 3, 12 },  // Chapter 3 có 12 level
    };

    private void Start()
    {
        GenerateLevels();
    }

    private void GenerateLevels()
    {
        int levelIndex = 1; // Đánh số level toàn game

        for (int chapter = 0; chapter < content.childCount; chapter++)
        {
            Transform panel = content.GetChild(chapter);

            // Lấy số lượng level của chapter hiện tại
            int levelCount = _chapters[chapter, 1];

            for (int i = 0; i < levelCount; i++)
            {
                int currentLevel = levelIndex;

                // Tạo button trong panel
                GameObject buttonObj = Instantiate(levelButtonPrefab, panel);
                buttonObj.name = $"Level_{currentLevel}";

                // Set text cho nút
                Text btnText = buttonObj.GetComponentInChildren<Text>();
                if (btnText != null)
                    btnText.text = currentLevel.ToString();

                // Thêm sự kiện click
                Button btn = buttonObj.GetComponent<Button>();
                btn.onClick.AddListener(() => OnLevelSelected(currentLevel));

                levelIndex++;
            }
        }
    }

    private void OnLevelSelected(int level)
    {
        Debug.Log($"Selected Level: {level}");
        // TODO: Load scene hoặc gameplay tương ứng
    }
}
