using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AudioSource))]
public class EnlargeHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    [Header("Audio Settings")]
    [SerializeField] private string audioClipName = "SpringSound"; // Name of the MP3 file in the Resources folder

    private Vector3 _originalScale;
    private Tween _currentTween;
    private AudioSource _audioSource;
    private AudioClip _audioClip;

    private void Awake()
    {
        // Store original scale
        _originalScale = transform.localScale;

        // Get the AudioSource component
        _audioSource = GetComponent<AudioSource>();

        // Load the audio clip from the Resources folder
        _audioClip = Resources.Load<AudioClip>(audioClipName);
        if (_audioClip != null)
        {
            _audioSource.clip = _audioClip;
        }
        else
        {
            Debug.LogError($"Audio clip '{audioClipName}' not found in Resources folder.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play the audio clip
        if (_audioClip != null)
        {
            _audioSource.Play();
        }

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
