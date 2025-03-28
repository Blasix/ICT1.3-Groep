using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StickerSceneManager : MonoBehaviour
{
    public GameObject stickerDog;
    public GameObject stickerCat;
    public GameObject stickerElephant;

    public RectTransform canvasRect;

    public Vector2 spawnLocationDog;
    public Vector2 spawnLocationCat;
    public Vector2 spawnLocationElephant;

    private GameObject activeSticker = null;
    private List<AppointmentItem> _appointmentsList;
    private ApiClient _apiClient;

    public string childName;
    private int lastCompletedLevel;

    void Start()
    {
        _apiClient = new ApiClient();
        childName = PlayerPrefs.GetString("SelectedChildName");
        LoadAppointments();
    }

    private async void LoadAppointments()
    {
        _appointmentsList = await GetAppointments();
        CheckCompletedLevels(_appointmentsList);
        DisplayStickers();
    }

    private async Task<List<AppointmentItem>> GetAppointments()
    {
        _appointmentsList = await _apiClient.GetAppointments(childName);
        return _appointmentsList;
    }

    // Checkt bij welk level het kind is
    void CheckCompletedLevels(List<AppointmentItem> appointments)
    {
        foreach (var appointment in appointments)
        {
            if (appointment.statusLevel == "completed")
            {
                switch (appointment.appointmentName)
                {
                    case "De ontmoeting":
                        lastCompletedLevel = 1;
                        break;
                    case "Voorbereiding":
                        lastCompletedLevel = 2;
                        break;
                    case "De operatie":
                        lastCompletedLevel = 3;
                        break;
                }
            }
        }
    }

    // Spawnt stickers obv welk level het kind is
    void DisplayStickers()
    {
        ClearSticker();

        // Show stickers based on completed levels
        switch (lastCompletedLevel)
        {
            case 1:
                SpawnSticker(stickerDog, spawnLocationDog);
                break;
            case 2:
                SpawnSticker(stickerDog, spawnLocationDog);
                SpawnSticker(stickerCat, spawnLocationCat);
                break;
            case 3:
                SpawnSticker(stickerDog, spawnLocationDog);
                SpawnSticker(stickerCat, spawnLocationCat);
                SpawnSticker(stickerElephant, spawnLocationElephant);
                break;
        }
    }

    // Method die stickers spawnt
    private void SpawnSticker(GameObject prefabToSpawn, Vector2 spawnLocation)
    {
        if (prefabToSpawn != null && canvasRect != null)
        {
            Vector2 anchoredPosition = new Vector2(
                spawnLocation.x * canvasRect.rect.width,
                spawnLocation.y * canvasRect.rect.height
            );

            GameObject spawnedPrefab = Instantiate(prefabToSpawn, canvasRect);
            RectTransform prefabRect = spawnedPrefab.GetComponent<RectTransform>();

            if (prefabRect != null)
            {
                prefabRect.anchoredPosition = anchoredPosition;
                activeSticker = spawnedPrefab;
            }
            else
            {
                Debug.LogError("Prefab does not have a RectTransform component.");
                Destroy(spawnedPrefab);
            }
        }
        else
        {
            Debug.LogError("Canvas RectTransform is not assigned.");
        }
    }

    private void ClearSticker()
    {
        if (activeSticker != null)
        {
            Destroy(activeSticker);
        }
        activeSticker = null;
    }
}
