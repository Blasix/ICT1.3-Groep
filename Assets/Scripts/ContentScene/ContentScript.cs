using System;
using System.Collections.Generic;
using UnityEngine;
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

        LoadContentItems();
        SortContentItems();
        UpdateTextContent();

        LeftButton.onClick.AddListener(MoveLeft);
        RightButton.onClick.AddListener(MoveRight);
    }

    private void LoadContentItems()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("ChildContent");
        if (jsonTextFile != null)
        {
            ChildContentWrapper contentWrapper = JsonUtility.FromJson<ChildContentWrapper>(jsonTextFile.text);
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
            TextContent.GetComponent<Text>().text = _currentItem.Text;
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
}
