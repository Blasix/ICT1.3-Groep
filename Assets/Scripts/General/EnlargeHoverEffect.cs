using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class EnlargeHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private Vector3 _originalScale;
    private Tween _currentTween;

    private void Awake()
    {
        // Store original scale
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Kill any active tween first
        _currentTween?.Kill();

        // Scale up effect
        _currentTween = transform.DOScale(_originalScale * hoverScale, scaleDuration)
            .SetEase(scaleEase)
            .SetUpdate(true); // SetUpdate(true) makes it work even when game is paused
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Kill any active tween first
        _currentTween?.Kill();

        // Return to original scale
        _currentTween = transform.DOScale(_originalScale, scaleDuration)
            .SetEase(scaleEase)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        // Reset scale when disabled
        transform.localScale = _originalScale;
        _currentTween?.Kill();
    }
}