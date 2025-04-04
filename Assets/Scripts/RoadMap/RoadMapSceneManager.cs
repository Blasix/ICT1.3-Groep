using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoadMapSceneManager : MonoBehaviour
{
    private string _childTraject;
    private string _selectedChildName;
    private string _selectedTrack;

    private string _prefabName;

    public Transform RoadmapContainerA;
    public Transform RoadmapContainerB;

    public Transform avatars;

    public Transform AAvatarAndCar;
    public Transform BAvatarAndCar;

    public GameObject EindScherm;

    private ApiClient _apiClient;

    private List<AppointmentItem> _appointments;

    private LevelCompletionData _levelCompletionData;

    public void Start()
    {
        _apiClient = new ApiClient();
        _childTraject = PlayerPrefs.GetString("SelectedTrajectId");
        _selectedChildName = PlayerPrefs.GetString("SelectedChildName");
        bool firstBoot = PlayerPrefs.GetString("isFirstBoot", "true") == "true";
        EindScherm.SetActive(false);
        if (firstBoot)
        {
            ResetAllLevelsToIncomplete();
            PlayerPrefs.SetString("isFirstBoot", "false");
        }
        RoadmapContainerA.gameObject.SetActive(false);
        RoadmapContainerB.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(_childTraject))
        {
            if (_childTraject == "95967735-0d27-4c36-9818-5b00b77ce5a9")
            {
                RoadmapContainerA.gameObject.SetActive(true);
                RoadmapContainerB.gameObject.SetActive(false);
                Debug.Log("SELECTED TRACK A");
                PlayerPrefs.SetString("SelectedTrajectName", "A");
                _selectedTrack = "A";
            }
            else if (_childTraject == "15967735-0d27-4c36-9818-5b00b77ce5a9")
            {
                RoadmapContainerB.gameObject.SetActive(true);
                RoadmapContainerA.gameObject.SetActive(false);
                Debug.Log("SELECTED TRACK B");
                PlayerPrefs.SetString("SelectedTrajectName", "B");
                _selectedTrack = "B";
            }
        }

        LoadLevelCompletionData();

        int i = 1;
        Debug.Log("Setting status colors");
        foreach (Transform child in RoadmapContainerA)
        {
            SetStatusColor(RoadmapContainerA, $"Step-{i}", GetCompletionStatus("trajectA", i));
            i++;
        }

        i = 1;
        foreach (Transform child in RoadmapContainerB)
        {
            SetStatusColor(RoadmapContainerB, $"Step-{i}", GetCompletionStatus("trajectB", i));
            i++;
        }
        SetAvatar();
        SetupLevelCompletion();
        GetAppointments();
    }
    
    public async void GetAppointments()
    {
        ApiClient apiClient = new ApiClient();
        _appointments = await apiClient.GetAppointments(_selectedChildName);
        if (_appointments != null)
        {
            setAppointmentsToItems(_appointments);
        }
        else
        {
            Debug.LogError("Failed to retrieve appointments.");
        }
    }


public void setAppointmentsToItems(List<AppointmentItem> appointments)
    {
        Transform activeContainer = null;
        if (_childTraject == "95967735-0d27-4c36-9818-5b00b77ce5a9")
        {
            activeContainer = RoadmapContainerA;
        }
        else
        {
            activeContainer = RoadmapContainerB;
        }


        foreach (Transform item in activeContainer)
        {
            foreach (var appointment in appointments)
            {
                if (item.name == $"Step-{appointment.LevelStep}-Date")
                {
                    // Set appointment details to the item
                    // Assuming the item has a TextMeshProUGUI component to display the appointment date
                    TextMeshProUGUI itemText = item.GetComponentInChildren<TextMeshProUGUI>();
                    if (itemText != null)
                    {
                        if (DateTime.TryParseExact(appointment.date, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime appointmentDate))
                        {
                            TimeSpan timeUntilAppointment = appointmentDate - DateTime.Now;
                            int daysUntilAppointment = (int)timeUntilAppointment.TotalDays;
                            if (daysUntilAppointment > 1)
                            {
                                itemText.text = $"Nog {daysUntilAppointment} nachtjes";
                            }
                            else if (daysUntilAppointment == 1)
                            {
                                itemText.text = $"Nog {daysUntilAppointment} nachtje";
                            }
                            else if (daysUntilAppointment == 0)
                            {
                                itemText.text = "Vandaag is de afspraak!";
                            }
                            else
                            {
                                itemText.text = "de afspraak is al geweest";
                            }
                        }
                        else
                        {
                            itemText.text = "Ongeldige datum";
                        }
                    }
                }
            }
        }
    }

    private void LoadLevelCompletionData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/LevelCompletion.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _levelCompletionData = JsonUtility.FromJson<LevelCompletionData>(json);
        }
        else
        {
            Debug.LogError("LevelCompletion.json file not found.");
        }
    }

    private void ResetAllLevelsToIncomplete()
    {
        if (_levelCompletionData != null)
        {
            foreach (var level in _levelCompletionData.trajectA)
            {
                level.CompletionStatus = "incomplete";
            }
            if (_levelCompletionData.trajectA.Count > 0)
            {
                _levelCompletionData.trajectA[0].CompletionStatus = "doing";
            }

            foreach (var level in _levelCompletionData.trajectB)
            {
                level.CompletionStatus = "incomplete";
            }
            if (_levelCompletionData.trajectB.Count > 0)
            {
                _levelCompletionData.trajectB[0].CompletionStatus = "doing";
            }

            SaveLevelCompletionData();
        }
    }

    public string GetCompletionStatus(string traject, int step)
    {
        if (_levelCompletionData != null)
        {
            List<LevelCompletion> levels = traject == "trajectA" ? _levelCompletionData.trajectA : _levelCompletionData.trajectB;
            if (step - 1 < levels.Count)
            {
                return levels[step - 1].CompletionStatus;
            }
        }
        return "incomplete";
    }

    private void SetAvatar()
    {
        _prefabName = PlayerPrefs.GetString("avatar_ID", "Avatar-1");

        if (!string.IsNullOrEmpty(_prefabName))
        {
            Sprite avatarSprite = null;

            // Find the avatar with the matching name
            foreach (Transform child in avatars)
            {
                if (child.gameObject.name == _prefabName)
                {
                    SpriteRenderer avatarSpriteRenderer = child.GetComponent<SpriteRenderer>();
                    if (avatarSpriteRenderer != null)
                    {
                        avatarSprite = avatarSpriteRenderer.sprite;
                        Debug.Log($"Avatar with name {_prefabName} found.");
                    }
                    break;
                }
            }

            if (avatarSprite != null)
            {
                // Set the sprite of children in AAvatarAndCar with names starting with "AvatarStep"
                foreach (Transform child in AAvatarAndCar)
                {
                    if (child.name.StartsWith("AvatarStep"))
                    {
                        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = avatarSprite;
                        }
                    }
                }

                // Set the sprite of children in BAvatarAndCar with names starting with "AvatarStep"
                foreach (Transform child in BAvatarAndCar)
                {
                    if (child.name.StartsWith("AvatarStep"))
                    {
                        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = avatarSprite;
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("No avatar sprite found for the given name.");
            }
        }
        else
        {
            Debug.LogWarning("No avatar name found in PlayerPrefs.");
        }
    }

    private void SetupLevelCompletion()
    {
        Debug.Log("Setting up level completion");

        int currentStep = -1;
        bool allLevelsCompleted = true;

        for (int i = 1; i <= (_selectedTrack == "A" ? _levelCompletionData.trajectA.Count : _levelCompletionData.trajectB.Count); i++)
        {
            string completionStatus = GetCompletionStatus(_selectedTrack == "A" ? "trajectA" : "trajectB", i);
            if (completionStatus == "doing")
            {
                currentStep = i;
                allLevelsCompleted = false;
                break;
            }
            if (completionStatus != "completed")
            {
                allLevelsCompleted = false;
            }
        }

        if (currentStep != -1)
        {
            for (int i = 1; i < currentStep; i++)
            {
                if (_selectedTrack == "A")
                {
                    SetStatusColor(RoadmapContainerA, $"Step-{i}", "completed");
                }
                else if (_selectedTrack == "B")
                {
                    SetStatusColor(RoadmapContainerB, $"Step-{i}", "completed");
                }
            }
        }

        for (int i = 1; i <= (_selectedTrack == "A" ? _levelCompletionData.trajectA.Count : _levelCompletionData.trajectB.Count); i++)
        {
            string completionStatus = GetCompletionStatus(_selectedTrack == "A" ? "trajectA" : "trajectB", i);
            if (_selectedTrack == "A")
            {
                SetStatusColor(RoadmapContainerA, $"Step-{i}", completionStatus);
                if (completionStatus == "doing")
                {
                    Debug.Log("SETTING CAR AND AVATAR");
                    SetActiveStateByName(AAvatarAndCar, $"CarStep{i}", true);
                    SetActiveStateByName(AAvatarAndCar, $"AvatarStep{i}", true);
                }
                else
                {
                    SetActiveStateByName(AAvatarAndCar, $"CarStep{i}", false);
                    SetActiveStateByName(AAvatarAndCar, $"AvatarStep{i}", false);
                }
            }
            else if (_selectedTrack == "B")
            {
                SetStatusColor(RoadmapContainerB, $"Step-{i}", completionStatus);
                if (completionStatus == "doing")
                {
                    Debug.Log("SETTING CAR AND AVATAR");
                    SetActiveStateByName(BAvatarAndCar, $"CarStep{i}", true);
                    SetActiveStateByName(BAvatarAndCar, $"AvatarStep{i}", true);
                }
                else
                {
                    SetActiveStateByName(BAvatarAndCar, $"CarStep{i}", false);
                    SetActiveStateByName(BAvatarAndCar, $"AvatarStep{i}", false);
                }
            }
        }

        if (allLevelsCompleted)
        {
            Debug.Log("All levels completed. Showing pop-up.");
            EindScherm.SetActive(true);
        }
    }

    private void UpdateLevelCompletionData(string traject, int step, string status)
    {
        if (_levelCompletionData != null)
        {
            List<LevelCompletion> levels = traject == "trajectA" ? _levelCompletionData.trajectA : _levelCompletionData.trajectB;
            if (step - 1 < levels.Count)
            {
                levels[step - 1].CompletionStatus = status;
            }
        }
    }

    private void SaveLevelCompletionData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/LevelCompletion.json");
        string json = JsonUtility.ToJson(_levelCompletionData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("LevelCompletion.json file updated.");
    }

    private void SetStatusColor(Transform container, string levelName, string status)
    {
        foreach (Transform child in container)
        {
            if (child.name == levelName)
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                {
                    switch (status.ToLower())
                    {
                        case "completed":
                            image.color = Color.green;
                            break;
                        case "doing":
                            image.color = Color.blue;
                            break;
                        case "incomplete":
                            image.color = Color.gray;
                            break;
                        default:
                            Debug.LogWarning($"Unknown status: {status}");
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning($"No Image component found on {child.name}");
                }
            }
        }
    }

    private void SetActiveStateByName(Transform container, string activeName, bool state)
    {
        bool found = false;
        foreach (Transform child in container)
        {
            if (child.name == activeName)
            {
                child.gameObject.SetActive(state);
                found = true;
            }
        }
        if (!found)
        {
            Debug.LogWarning($"GameObject with name {activeName} not found in {container.name}");
        }
    }

    public void OnLevelClick(GameObject level)
    {
        PlayerPrefs.SetString("SelectedLevel", level.name);
        SceneManager.LoadScene("InformationPanel");
    }

    public void OnLogoutButtonClicked()
    {
        PlayerPrefs.DeleteAll();
        // ResetAllLevelsToIncomplete();
        SceneManager.LoadScene("WelcomeScene");
    }

    public void OnAvatarButtonClicked()
    {
        SceneManager.LoadScene("AvatarScene");
    }

    public void OnJournalButtonClick()
    {
        SceneManager.LoadScene("JournalScene");
    }

    public void UpdateLevelStatus(string traject, int step, string newStatus)
    {
        // Update the LevelCompletionData
        UpdateLevelCompletionData(traject, step, newStatus);

        // Update the corresponding AppointmentItem
        foreach (var appointment in _appointments)
        {
            if (appointment.LevelStep == step)
            {
                appointment.statusLevel = newStatus;
                break;
            }
        }

        // Save the updated JSON file
        SaveLevelCompletionData();

        // Optionally, update the UI
        if (traject == "A")
        {
            SetStatusColor(RoadmapContainerA, $"Step-{step}", newStatus);
        }
        else if (traject == "B")
        {
            SetStatusColor(RoadmapContainerB, $"Step-{step}", newStatus);
        }

        Debug.Log($"Level {step} in traject {traject} updated to {newStatus}");
    }

    public void OnAfsluitenButtonPresse()
    {
        EindScherm.SetActive(false);
    }

    public void OnStickerClick()
    {
        SaveCompletedLevelsCountToPlayerPrefs();
        SceneManager.LoadScene("StickerScene");
    }

    private void SaveCompletedLevelsCountToPlayerPrefs()
    {
        int completedLevelsCount = 0;

        foreach (var level in _levelCompletionData.trajectA)
        {
            if (level.CompletionStatus == "completed")
            {
                completedLevelsCount++;
            }
        }

        foreach (var level in _levelCompletionData.trajectB)
        {
            if (level.CompletionStatus == "completed")
            {
                completedLevelsCount++;
            }
        }

        PlayerPrefs.SetInt("CompletedLevelsCount", completedLevelsCount);
        PlayerPrefs.Save();
        Debug.Log($"Completed levels count ({completedLevelsCount}) saved to PlayerPrefs.");
    }

}

[System.Serializable]
public class LevelCompletionData
{
    public List<LevelCompletion> trajectA;
    public List<LevelCompletion> trajectB;
}

[System.Serializable]
public class LevelCompletion
{
    public string LevelName;
    public string CompletionStatus;
}
