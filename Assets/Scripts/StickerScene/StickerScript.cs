using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerScript : MonoBehaviour
{
    public Image[] Stickers;  // Array to store sticker images
    public string TrajectId = "95967735-0d27-4c36-9818-5b00b77ce5a9";
    private List<AppointmentItem> appointments;

    void Start()
    {
        if (TrajectId == "95967735-0d27-4c36-9818-5b00b77ce5a9")
        {
            HideStickers(6, 8);  // Hide Sticker7 and Sticker8
        }

        GetAppointments();
    }

    private void HideStickers(int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            Stickers[i]?.gameObject.SetActive(false);
        }
    }

    public async void GetAppointments()
    {
        ApiClient apiClient = new ApiClient();
        string childName = "Bob";
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

        foreach (AppointmentItem appointment in appointments)
        {
            if (appointment.statusLevel == "Completed")
            {
                int index = appointment.LevelStep - 1;
                if (index >= 0 && index < Stickers.Length)
                {
                    Stickers[index]?.gameObject.SetActive(true);
                }
            }
        }
    }
}
