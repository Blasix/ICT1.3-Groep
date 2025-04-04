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
        ChildTraject = PlayerPrefs.GetString("SelectedTrajectId");
        
        HideAllStickers();  // Ensure all stickers are hidden at the start

        if (ChildTraject == "95967735-0d27-4c36-9818-5b00b77ce5a9")
        {
            HideStickers(6, 8);  // Hide Sticker7 and Sticker8
        }

        GetAppointments();
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

    private void HideStickers(int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i >= 0 && i < Stickers.Length && Stickers[i] != null)
            {
                Stickers[i].gameObject.SetActive(false);
            }
        }
    }

    public async void GetAppointments()
    {
        ApiClient apiClient = new ApiClient();
        string childName = PlayerPrefs.GetString("SelectedChildName");
        appointments = await apiClient.GetAppointments(childName);

        if (appointments == null)
        {
            Debug.LogError("Failed to retrieve appointments.");
            return;
        }

        StickerCheck();
    }

    private void StickerCheck()
    {
        if (appointments == null) return;

        HideAllStickers();  // First, hide all stickers

        foreach (AppointmentItem appointment in appointments)
        {
            if (appointment.statusLevel == "Completed")
            {
                int index = appointment.LevelStep - 1;
                if (index >= 0 && index < Stickers.Length && Stickers[index] != null)
                {
                    Stickers[index].gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnRoadMapBtnClick()
    {
        SceneManager.LoadScene("RoadMapScene");
    }
}
