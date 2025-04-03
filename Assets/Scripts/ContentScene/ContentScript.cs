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

    public Button LeftButton;
    public Button RightButton;

    private string _currentTraject;
    private string _currentLevel;
    private int _currentIndex = 0;
    private ContentItem _currentItem;
    private List<ContentItem> _contentItems;

    public void Start()
    {
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
        UpdateTextContentById(ConvertLevelToId(_currentLevel));
        LeftButton.onClick.AddListener(MoveLeft);
        RightButton.onClick.AddListener(MoveRight);
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

    public void MoveLeft()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            UpdateTextContent();
        }
    }

    public void MoveRight()
    {
        if (_currentIndex < _contentItems.Count - 1)
        {
            _currentIndex++;
            UpdateTextContent();
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

    public void OnHomeButtonPressed()
    {
        SceneManager.LoadScene("RoadmapScene");
    }
}
