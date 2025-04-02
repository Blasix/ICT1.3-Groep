using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static ApiClient;

public class Contentascript : MonoBehaviour
{
    private string trajectid;
    private int Step;
    private List<Level> Levels;
    public GameObject ContentBlock;
    public Transform contentContainer;
    public Button forwardButton;
    public Button backwardButton;
    private int currentIndex = 0;

    void Start()
    {
        trajectid = PlayerPrefs.GetString("SelectedTrajectId");
        Step = PlayerPrefs.GetInt("step");
        GetLevelContent();
        forwardButton.onClick.AddListener(NextContent);
        backwardButton.onClick.AddListener(PreviousContent);

        ApiClient apiClient = new ApiClient();
        string statusLevel = "doing";
        string childId = PlayerPrefs.GetString("SelectedChildId");
        apiClient.UpdateAppointment(childId, Step, statusLevel);
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
            Button videobutton = contentBlockInstance.GetComponentInChildren<Button>();
            TextMeshProUGUI textBlock = contentBlockInstance.GetComponentInChildren<TextMeshProUGUI>(); // Change to TextMeshProUGUI

            int index = i;
            if (!string.IsNullOrEmpty(Levels[index].Url))
            {
                videobutton.onClick.AddListener(() => Application.OpenURL(Levels[index].Url));
                videobutton.gameObject.SetActive(true);
                textBlock.gameObject.SetActive(false);
            }
            else if (!string.IsNullOrEmpty(Levels[index].Tekst))
            {
                textBlock.text = Levels[index].Tekst;
                textBlock.gameObject.SetActive(true);
                videobutton.gameObject.SetActive(false);
            }

            contentBlockInstance.SetActive(index == currentIndex); // Show only the first content block initially
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
            // Make an API request to update the appointment status
            ApiClient apiClient = new ApiClient();
            string statusLevel = "completed";
            string childId = PlayerPrefs.GetString("SelectedChildId");
            apiClient.UpdateAppointment(childId, Step, statusLevel);

            SceneManager.LoadScene("RoadMapScene");
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
