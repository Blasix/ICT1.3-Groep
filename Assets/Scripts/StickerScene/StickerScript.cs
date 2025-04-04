using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StickerScript : MonoBehaviour
{
    public Image[] Stickers;  // Array to store sticker images
    public string ChildTraject;
    private List<AppointmentItem> appointments;
    public Button RoadMapBtn;

    void Start()
    {
        int levels = PlayerPrefs.GetInt("CompletedLevelsCount", 0);
        HideAllStickers();  // Ensure all stickers are hidden at the start
        ShowStickers(levels);  // Show the appropriate number of stickers
        RoadMapBtn.onClick.AddListener(OnRoadMapBtnClick);
    }

    private void HideAllStickers()
    {
        foreach (var sticker in Stickers)
        {
            if (sticker != null)
                sticker.gameObject.SetActive(false);
        }
    }

    private void ShowStickers(int levels)
    {
        for (int i = 0; i < levels && i < Stickers.Length; i++)
        {
            if (Stickers[i] != null)
                Stickers[i].gameObject.SetActive(true);
        }
    }

    public void OnRoadMapBtnClick()
    {
        SceneManager.LoadScene("RoadMapScene");
    }
}
