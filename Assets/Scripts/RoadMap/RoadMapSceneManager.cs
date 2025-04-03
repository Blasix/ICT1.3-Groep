using System.Collections.Generic;
using System.Threading.Tasks;
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

    private ApiClient _apiClient;

    private List<AppointmentItem> _appointments;

    public void Start()
    {
        _apiClient = new ApiClient();
        _childTraject = PlayerPrefs.GetString("SelectedTrajectId");
        _selectedChildName = PlayerPrefs.GetString("SelectedChildName");

        RoadmapContainerA.gameObject.SetActive(false);
        RoadmapContainerB.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(_childTraject))
        {
            if (_childTraject == "95967735-0d27-4c36-9818-5b00b77ce5a9")
            {
                RoadmapContainerA.gameObject.SetActive(true);
                RoadmapContainerB.gameObject.SetActive(false);
                Debug.Log("SELECTED TRACK A");
                _selectedTrack = "A";
            }
            else if (_childTraject == "15967735-0d27-4c36-9818-5b00b77ce5a9")
            {
                RoadmapContainerB.gameObject.SetActive(true);
                RoadmapContainerA.gameObject.SetActive(false);
                Debug.Log("SELECTED TRACK B");
                _selectedTrack = "B";
            }
        }

        int i = 1;

        foreach (Transform child in RoadmapContainerA)
        {
            Debug.Log($"Setting status color of Step-{i}");
            SetStatusColor(RoadmapContainerA, $"Step-{i}", "incomplete");
            i++;
        }

        foreach (Transform child in RoadmapContainerB)
        {
            Debug.Log($"Setting status color of Step-{i}");
            SetStatusColor(RoadmapContainerA, $"Step-{i}", "incomplete");
            i++;
        }
        SetAvatar();
        SetupAppointments();
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
                            Debug.Log($"Set sprite of {child.name} in AAvatarAndCar to avatar sprite.");
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
                            Debug.Log($"Set sprite of {child.name} in BAvatarAndCar to avatar sprite.");
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

    private async void SetupAppointments()
    {
        Debug.Log("Setting up appointments");
        _appointments = await _apiClient.GetAppointments(_selectedChildName);

        int currentStep = -1;

        foreach (AppointmentItem appointment in _appointments)
        {
            Debug.Log("Iterating through appointments");
            string appointmentCompletion = appointment.statusLevel;
            int step = appointment.LevelStep;

            if (appointmentCompletion == "doing")
            {
                currentStep = step;
                break;
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

        foreach (AppointmentItem appointment in _appointments)
        {
            string appointmentCompletion = appointment.statusLevel;
            int step = appointment.LevelStep;
            Debug.Log("SELECTED TRACK IS" + _selectedTrack);
            if (_selectedTrack == "A")
            {
                SetStatusColor(RoadmapContainerA, $"Step-{appointment.LevelStep}", appointment.statusLevel);
                if (appointmentCompletion == "doing")
                {
                    Debug.Log("SETTING CAR AND AVATAR");
                    SetActiveStateByName(AAvatarAndCar, $"CarStep{step}", true);
                    SetActiveStateByName(AAvatarAndCar, $"AvatarStep{step}", true);
                }
            }
            else if (_selectedTrack == "B")
            {
                SetStatusColor(RoadmapContainerB, $"Step-{appointment.LevelStep}", appointment.statusLevel);
                if (appointmentCompletion == "doing")
                {
                    Debug.Log("SETTING CAR AND AVATAR");
                    SetActiveStateByName(BAvatarAndCar, $"CarStep{step}", true);
                    SetActiveStateByName(BAvatarAndCar, $"AvatarStep{step}", true);
                }
            }
        }
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
                    Debug.Log($"Set color of {child.name} to {image.color} based on status {status}");
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
                Debug.Log($"Set {child.name} active state to {state}");
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
}
