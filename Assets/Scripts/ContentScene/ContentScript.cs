using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
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

    private string _currentTraject;
    private string _currentLevel;
    private int _currentIndex = 0;
    private ContentItem _currentItem;
    private List<ContentItem> _contentItems;
    private int _clickCount = 1;

    public void Start()
    {
        Window1.SetActive(true);
        Window2.SetActive(false);
        _currentLevel = PlayerPrefs.GetString("SelectedLevel");
        if (PlayerPrefs.GetString("SelectedTrajectId") == "15967735-0d27-4c36-9818-5b00b77ce5a9")
        {
            _currentTraject = "A";
        }
        else if (PlayerPrefs.GetString("SelectedTrajectId") == "95967735-0d27-4c36-9818-5b00b77ce5a9")
        {
            _currentTraject = "B";
        }

        Debug.Log("Loading content items");
        LoadContentItems();
        Debug.Log("Sorting content items");
        SortContentItems();
        Debug.Log("Updating text content");
        UpdateTextContent();

        LeftButton.onClick.AddListener(OnBackButtonPressed);
        RightButton.onClick.AddListener(OnContinueButtonPressed);
        HomeButton.onClick.AddListener(OnHomeButtonPressed);
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

    private void SortContentItems()
    {
        _contentItems.Sort((x, y) => x.SortingOrder.CompareTo(y.SortingOrder));
    }

    private void UpdateTextContent()
    {
        if (_contentItems != null && _contentItems.Count > 0 && _currentIndex >= 0 && _currentIndex < _contentItems.Count)
        {
            _currentItem = _contentItems[_currentIndex];
            TextMeshProUGUI textComponent = TextContent.GetComponent<TextMeshProUGUI>();
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
            // Close the window and navigate back to the roadmap scene
            Window1.SetActive(false);
            Window2.SetActive(false);
            SceneManager.LoadScene("RoadmapScene");
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
}
