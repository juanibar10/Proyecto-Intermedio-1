using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Color pressedColor = Color.gray;
    [SerializeField] private Vector3 pressedTextOffset = new(0f, -2f, 0f);

    private Button _button;
    private RectTransform _textRect;
    
    private Vector3 _originalTextPos;
    private Color _originalColor;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _textRect = buttonText.GetComponent<RectTransform>();
        
        _originalTextPos = _textRect.localPosition;
        _originalColor = buttonText.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable) return;

        buttonText.color = pressedColor;
        _textRect.localPosition = _originalTextPos + pressedTextOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_button.interactable) return;

        buttonText.color = _originalColor;
        _textRect.localPosition = _originalTextPos;
    }
}