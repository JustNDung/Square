using UnityEngine;

public class UIHelpers : MonoBehaviour
{
    [SerializeField] private GameObject popUpTextPrefab;
    [SerializeField] private Canvas overlayCanvas; // Screen Space - Overlay Canvas

    public static UIHelpers Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Tạo popup text ở giữa màn hình hoặc tại vị trí cụ thể (UI space)
    /// </summary>
    /// <param name="text">Nội dung hiển thị</param>
    /// <param name="screenPosition">Vị trí màn hình (nếu null thì mặc định là giữa màn hình)</param>
    public void ShowPopUpUI(string text, Vector2? screenPosition = null)
    {
        if (popUpTextPrefab == null || overlayCanvas == null)
        {
            Debug.LogWarning("PopUpText prefab hoặc Canvas chưa được gán!");
            return;
        }

        Vector2 spawnPos = screenPosition ?? new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Instantiate object as child of the canvas
        GameObject popup = Instantiate(popUpTextPrefab, overlayCanvas.transform);
        
        // Set position via RectTransform anchoredPosition
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                overlayCanvas.transform as RectTransform,
                spawnPos,
                overlayCanvas.worldCamera, // can be null for Overlay mode
                out anchoredPos
            );
            rectTransform.anchoredPosition = anchoredPos;
        }

        // Setup popup text
        PopUpText popUpComponent = popup.GetComponent<PopUpText>();
        if (popUpComponent != null)
        {
            popUpComponent.SetupText(text);
        }
    }
    
    /// <summary>
    /// Chuyển đổi screen position (ví dụ: vị trí chuột) sang UI local position trong canvas, có thể thêm offset
    /// </summary>
    /// <param name="screenPosition">Vị trí trên màn hình (ví dụ: Input.mousePosition)</param>
    /// <param name="offset">Độ lệch x/y nếu muốn hiển thị lệch đi (ví dụ: new Vector2(50, 30))</param>
    /// <returns>UI anchoredPosition trong canvas</returns>
    public Vector2 ConvertScreenToUIPosition(Vector2 screenPosition, Vector2 offset = default)
    {
        Vector2 localPoint = Vector2.zero;

        if (overlayCanvas != null && overlayCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                overlayCanvas.transform as RectTransform,
                screenPosition + offset,
                null, // null cho Overlay mode
                out localPoint
            );
        }
        else if (overlayCanvas != null && overlayCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                overlayCanvas.transform as RectTransform,
                screenPosition + offset,
                overlayCanvas.worldCamera,
                out localPoint
            );
        }

        return localPoint;
    }
    
}