using UnityEngine;
using UnityEngine.UI;

public class SnapChapter : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private RectTransform sampleListItem;
    [SerializeField] private HorizontalLayoutGroup hlg;
    [SerializeField] private float snapForce;
    private float snapSpeed;
    private bool isSnaped = false;

    private void Start()
    {
        isSnaped = false;
    }

    private void Update()
    {
        int currentChapter = Mathf.RoundToInt(0 - contentPanel.localPosition.x / (sampleListItem.rect.width + hlg.spacing));

        if (scrollRect.velocity.magnitude < 200 && !isSnaped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, -currentChapter * (sampleListItem.rect.width + hlg.spacing), snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z);
            if (contentPanel.localPosition.x == -currentChapter * (sampleListItem.rect.width + hlg.spacing))
            {
                isSnaped = true;
            }
        }

        if (scrollRect.velocity.magnitude > 200)
        {
            isSnaped = false;
            snapSpeed = 0;
        }
    }
}
