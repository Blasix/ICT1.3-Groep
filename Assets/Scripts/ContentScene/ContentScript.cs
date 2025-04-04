using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContentScript : MonoBehaviour
{
    public GameObject TextContent;
    public GameObject Window1;
    public GameObject Window2;

    public Button LeftButton;
    public Button RightButton;
    public Button HomeButton;
    public Button LinkButton;

    private int _currentIndex = 0;
    private int _clickCount = 1;
    
    private string _prefabName;
    private string _currentTraject;
    private string _currentLevel;
    private string _currentLink;

    private ContentItem _currentItem;
    private List<ContentItem> _contentItems;
    public GameObject AvatarGameObject;
    public GameObject[] AvatarGameObjects;
    private Dictionary<string, List<Dictionary<string, string>>> _levelCompletionData;

    public void Start()
    {
        _prefabName = PlayerPrefs.GetString("avatar_ID", "Avatar-1");
        Sprite avatarSprite = null;

        // Find the avatar with the matching name
        foreach (GameObject avatarGameObject in AvatarGameObjects)
        {
            if (avatarGameObject != null && avatarGameObject.name == _prefabName)
            {
                SpriteRenderer avatarSpriteRenderer = avatarGameObject.GetComponent<SpriteRenderer>();
                if (avatarSpriteRenderer != null)
                {
                    avatarSprite = avatarSpriteRenderer.sprite;
                    AvatarGameObject.GetComponent<SpriteRenderer>().sprite = avatarSprite; // Set the sprite of AvatarGameObject
                }
            }
        }

        Window1.SetActive(true);
        Window2.SetActive(false);
        _currentLevel = PlayerPrefs.GetString("SelectedLevel");
        _currentTraject = PlayerPrefs.GetString("SelectedTrajectName");

        Debug.Log("Loading content items");
        LoadContentItems();
        Debug.Log("Updating text content");
        UpdateTextContent();

        LeftButton.onClick.AddListener(OnBackButtonPressed);
        RightButton.onClick.AddListener(OnContinueButtonPressed);
        HomeButton.onClick.AddListener(OnHomeButtonPressed);
        LinkButton.onClick.AddListener(OnLinkButtonPressed);


        LoadLevelCompletionData();
    }

    private string ConvertLevelToId(string levelId)
    {
        if (_currentTraject == "A")
        {
            Debug.Log($"Called with id: {levelId}");
            switch (levelId)
            {
                case "Step-1":
                    return "AankomstBijDeSpoedeisendeHulp";
                case "Step-2":
                    return "EersteBeoordeling";
                case "Step-3":
                    return "MedischeOnderzoeken";
                case "Step-4":
                    return "BehandelingMetGips";
                case "Step-5":
                    return "NazorgEnHerstel";
                case "Step-6":
                    return "Gipsverwijdering";
                default:
                    Debug.LogError($"Unknown levelId: {levelId}");
                    return "";
            }
        }
        else
        {
            Debug.Log($"Called with id: {levelId}");
            switch (levelId)
            {
                case "Step-1":
                    return "AankomstBijDeSpoedeisendeHulp";
                case "Step-2":
                    return "EersteBeoordeling";
                case "Step-3":
                    return "MedischeOnderzoeken";
                case "Step-4":
                    return "ConsultatieEnBesluitvorming";
                case "Step-5":
                    return "Operatieproces";
                case "Step-6":
                    return "HerstelNaOperatie";
                case "Step-7":
                    return "NazorgEnHerstel";
                case "Step-8":
                    return "GipsverwijderingEnVerdereZorg";
                default:
                    Debug.LogError($"Unknown levelId: {levelId}");
                    return "";
            }
        }
    }

    private void LoadContentItems()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("ChildContent");
        if (jsonTextFile != null)
        {
            ChildContentWrapper contentWrapper = JsonConvert.DeserializeObject<ChildContentWrapper>(jsonTextFile.text);
            if (_currentTraject == "A")
            {
                _contentItems = contentWrapper.TrajectA;
            }
            else if (_currentTraject == "B")
            {
                _contentItems = contentWrapper.TrajectB;
            }
        }
        else
        {
            Debug.LogError("ChildContent.json file not found in Resources folder.");
        }
    }

    private void UpdateTextContent()
    {
        if (_contentItems != null && _contentItems.Count > 0 && _currentIndex >= 0 && _currentIndex < _contentItems.Count)
        {
            _currentItem = _contentItems[_currentIndex];
            TextMeshProUGUI textComponent = TextContent.GetComponent<TextMeshProUGUI>();
            _currentLink = _currentItem.Link;
            Debug.Log($"Current link set to: {_currentLink}");
            if (textComponent != null)
            {
                textComponent.text = _currentItem.Text;
            }
            else
            {
                Debug.LogError("TextContent does not have a TextMeshProUGUI component");
            }
        }
    }

    public void UpdateTextContentById(string id)
    {
        ContentItem item = _contentItems.Find(x => x.id == id);
        if (item != null)
        {
            TextMeshProUGUI textComponent = TextContent.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                Debug.Log("text converted");
                textComponent.text = item.Text;
            }
            else
            {
                Debug.LogError("TextContent does not have a TextMeshProUGUI component");
            }
        }
        else
        {
            Debug.LogError($"ContentItem with id {id} not found");
        }
    }

    public void OnContinueButtonPressed()
    {
        _clickCount++;
        Debug.Log($"Click count: {_clickCount}");
        if (_clickCount == 1)
        {
            // Show the first window with the text from the JSON file
            Window1.SetActive(true);
            Window2.SetActive(false);
            UpdateTextContentById(ConvertLevelToId(_currentLevel));
        }
        else if (_clickCount == 2)
        {
            // Show the second window
            Window1.SetActive(false);
            Window2.SetActive(true);
        }
        else if (_clickCount == 3)
        {
            Debug.Log("Click count of 3 detected updating level status");
            UpdateLevelStatusIfDoing();
            // Close the window and navigate back to the roadmap scene
            Window1.SetActive(false);
            Window2.SetActive(false);
            SceneManager.LoadScene("RoadmapScene");
        }
    }

    private void LoadLevelCompletionData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/LevelCompletion.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _levelCompletionData = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);
        }
        else
        {
            Debug.LogError("LevelCompletion.json file not found.");
        }
    }

    private void SaveLevelCompletionData()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/LevelCompletion.json");
        string json = JsonConvert.SerializeObject(_levelCompletionData, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log("LevelCompletion.json file updated.");
    }

    private string GetCompletionStatus(string traject, int step)
    {
        if (_levelCompletionData != null)
        {
            List<Dictionary<string, string>> levels = traject == "A" ? _levelCompletionData["trajectA"] : _levelCompletionData["trajectB"];
            if (step - 1 < levels.Count)
            {
                return levels[step - 1]["CompletionStatus"];
            }
        }
        return "incomplete";
    }

    private void UpdateLevelStatus(string traject, int step, string newStatus)
    {
        Debug.Log($"Updating level status for {traject} step {step} to {newStatus}.");
        if (_levelCompletionData != null)
        {
            List<Dictionary<string, string>> levels = traject == "A" ? _levelCompletionData["trajectA"] : _levelCompletionData["trajectB"];
            if (step - 1 < levels.Count)
            {
                levels[step - 1]["CompletionStatus"] = newStatus;
                SaveLevelCompletionData();
            }
        }
        else
        {
            Debug.LogError("LevelCompletionData is null.");
        }
    }

    private void UpdateLevelStatusIfDoing()
    {
        // Get the current level step
        int step = int.Parse(_currentLevel.Split('-')[1]);

        // Check if the current level status is "doing"
        string currentStatus = GetCompletionStatus(_currentTraject, step);
        if (currentStatus == "doing")
        {
            // Update the current level status to "completed"
            UpdateLevelStatus(_currentTraject, step, "completed");

            // Update the next level status to "doing"
            UpdateLevelStatus(_currentTraject, step + 1, "doing");
        }
    }

    public void OnBackButtonPressed()
    {
        if (_clickCount == 2)
        {
            // Go back to the first window
            Window1.SetActive(true);
            Window2.SetActive(false);
            _clickCount--;
        }
        else if (_clickCount == 1)
        {
            // Go back to the roadmap scene
            SceneManager.LoadScene("RoadmapScene");
        }
    }

    public void OnHomeButtonPressed()
    {
        SceneManager.LoadScene("RoadmapScene");
    }

    public void OnLinkButtonPressed()
    {
        Debug.Log($"Link button pressed with link {_currentLink}");
        Application.OpenURL(_currentLink);
    }
}