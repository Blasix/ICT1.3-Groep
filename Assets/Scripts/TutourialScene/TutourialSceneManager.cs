using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutourialSceneManager : MonoBehaviour
{
    private int _windowNum = 0;

    public GameObject[] Windows;
    private GameObject _currentWindow;
    public Button BackButton;
    public GameObject Panel;
    public Sprite WoodBackroundSprite;
    public Sprite RoadmapBackroundSprite;

    private Image _panelImage;

    public void Start()
    {
        _panelImage = Panel.GetComponent<Image>();
        _currentWindow = Windows[0];
        _currentWindow.SetActive(true);
        BackButton.gameObject.SetActive(false);
        UpdateScene();
    }

    public void UpdateScene()
    {
        if (_windowNum == 0)
        {
            BackButton.gameObject.SetActive(false);
        }
        else if (_windowNum >= Windows.Length)
        {
            SceneManager.LoadScene("AvatarScene");
        }
        else if (_windowNum == 2)
        {
            _panelImage.sprite = WoodBackroundSprite;
        }
        else if (_windowNum == 3)
        {
            _panelImage.sprite = RoadmapBackroundSprite;
        }
        else if (_windowNum == 4)
        {
            _panelImage.sprite = WoodBackroundSprite;
        }
        else
        {
            BackButton.gameObject.SetActive(true);
        }
        _currentWindow.SetActive(false);
        _currentWindow = Windows[_windowNum];
        _currentWindow.SetActive(true);
    }

    public void OnContinueButonClick()
    {
        _currentWindow.SetActive(false);
        _windowNum++;
        UpdateScene();
    }

    public void OnBackButtonClick()
    {
        if (_windowNum > 0)
        {
            _currentWindow.SetActive(false);
            _windowNum--;
            UpdateScene();
        }
    }
}
