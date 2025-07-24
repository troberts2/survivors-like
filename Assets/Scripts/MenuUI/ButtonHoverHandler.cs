using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _rectTransform;
    [SerializeField] private float scaleChange = 0.1f;
    [SerializeField] private float timeToScale = .25f;
    private Vector3 _originalScale;

    /// <summary>
    /// gets variables
    /// </summary>
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = transform.localScale;
    }

    /// <summary>
    /// called on button hover
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        IncreaseButtonSize();
    }

    /// <summary>
    /// called on button hover exit
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        DecreaseButtonSize();
    }

    /// <summary>
    /// Increases button size animation
    /// </summary>
    private void IncreaseButtonSize()
    {
        _rectTransform.localScale = _originalScale;
        _rectTransform.DOScale(_rectTransform.localScale.x + scaleChange, timeToScale);
    }

    /// <summary>
    /// decreases button size animation
    /// </summary>
    private void DecreaseButtonSize()
    {
        _rectTransform.localScale = _originalScale * (1 + scaleChange);
        _rectTransform.DOScale(_originalScale, timeToScale);
    }
}