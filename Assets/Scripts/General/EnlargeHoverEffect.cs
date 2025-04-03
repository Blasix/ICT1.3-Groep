using UnityEngine;
using DG.Tweening;

public class EnlargeHoverEffect : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    private Vector3 _originalScale;
    private Tween _currentTween;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        // Kill any ongoing tween to prevent conflicts
        _currentTween?.Kill();

        // Scale up with DoTween
        _currentTween = transform.DOScale(_originalScale * hoverScale, scaleDuration)
            .SetEase(scaleEase);
    }

    private void OnMouseExit()
    {
        // Kill any ongoing tween to prevent conflicts
        _currentTween?.Kill();

        // Return to original scale
        _currentTween = transform.DOScale(_originalScale, scaleDuration)
            .SetEase(scaleEase);
    }

    private void OnDestroy()
    {
        // Clean up tweens when object is destroyed
        _currentTween?.Kill();
    }
}