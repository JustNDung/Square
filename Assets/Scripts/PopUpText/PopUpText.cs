using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{
    private TextMeshProUGUI _myText;
    private float _textTimer;
    
    [SerializeField] private float floatSpeed = 30f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float lifeTime = 1.5f;

    private RectTransform _rectTransform;

    void Awake()
    {
        _myText = GetComponent<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetupText(string text)
    {
        _myText.text = text;
        _myText.color = new Color(_myText.color.r, _myText.color.g, _myText.color.b, 1f);
        _textTimer = lifeTime;
    }

    private void Update()
    {
        // Move upward
        _rectTransform.anchoredPosition += Vector2.up * floatSpeed * Time.deltaTime;

        _textTimer -= Time.deltaTime;

        if (_textTimer < 0)
        {
            Color c = _myText.color;
            c.a -= fadeSpeed * Time.deltaTime;
            _myText.color = c;

            if (_myText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}