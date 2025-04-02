using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static ApiClient;

public class Contentascript : MonoBehaviour
{
    private string trajectid = "95967735-0D27-4C36-9818-5B00B77CE5A9";
    private int Step = 1;
    private List<Level> Levels;
    public GameObject ContentBlock;
    public Transform contentContainer;
    public Button forwardButton;
    public Button backwardButton;
    private int currentIndex = 0;

    void Start()
    {
        GetLevelContent();
        forwardButton.onClick.AddListener(NextContent);
        backwardButton.onClick.AddListener(PreviousContent);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void GetLevelContent()
    {
        ApiClient apiClient = new ApiClient();
        Levels = await apiClient.GetAllLevels(Step, trajectid);
        if (Levels != null && Levels.Count > 0)
        {
            LoadContentBlocks();
        }
        else
        {
            Debug.LogError("Failed to retrieve levels or no levels available.");
        }
    }

    private void LoadContentBlocks()
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            GameObject contentBlockInstance = Instantiate(ContentBlock, contentContainer);
            VideoPlayer videoPlayer = contentBlockInstance.GetComponentInChildren<VideoPlayer>();
            TextMeshProUGUI textBlock = contentBlockInstance.GetComponentInChildren<TextMeshProUGUI>(); // Change to TextMeshProUGUI

            if (!string.IsNullOrEmpty(Levels[i].Url))
            {
                videoPlayer.url = Levels[i].Url;
                videoPlayer.gameObject.SetActive(true);
                textBlock.gameObject.SetActive(false);
            }
            else if (!string.IsNullOrEmpty(Levels[i].Tekst))
            {
                textBlock.text = Levels[i].Tekst;
                textBlock.gameObject.SetActive(true);
                videoPlayer.gameObject.SetActive(false);
            }

            contentBlockInstance.SetActive(i == currentIndex); // Show only the first content block initially
        }
    }

    private void NextContent()
    {
        if (currentIndex < Levels.Count - 1)
        {
            contentContainer.GetChild(currentIndex).gameObject.SetActive(false);
            currentIndex++;
            contentContainer.GetChild(currentIndex).gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("RoadMapScene"); // Load the welcome scene when there is no more content
        }
    }

    private void PreviousContent()
    {
        if (currentIndex > 0)
        {
            contentContainer.GetChild(currentIndex).gameObject.SetActive(false);
            currentIndex--;
            contentContainer.GetChild(currentIndex).gameObject.SetActive(true);
        }
    }

    public void OnHomeBtn()
    {
        SceneManager.LoadScene("RoadMapScene");
    }
}
