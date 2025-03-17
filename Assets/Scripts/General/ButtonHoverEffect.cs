using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour
{
    public TMP_Text TMP_TextRegister;
    private string _defaulDarkBlueHex = "#333893";
    private string _hoverColorHex = "00FFF2";
    private Color _normalColor;
    private Color _hoverColor;
    void Start()
    {
        ColorUtility.TryParseHtmlString(_defaulDarkBlueHex, out _normalColor);
        ColorUtility.TryParseHtmlString(_hoverColorHex, out _hoverColor);
        TMP_TextRegister.color = _normalColor;
    }
    public void OnPointerEnter()
    {
        TMP_TextRegister.color = _hoverColor;
    }

    public void OnPointerExit()
    {
        TMP_TextRegister.color = _normalColor;
    }

    public void OnRegisterButtonClicked()
    {
        Debug.Log("Register Button Clicked");
    }
}

