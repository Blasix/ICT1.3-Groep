using Items;
using Newtonsoft.Json;
using Patient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChildCreation : MonoBehaviour
{
    public TMP_InputField NameInputField;
    public TMP_Text ErrorText;
    
    public Button CreatePatientButton;
    public Button BackButton;
    public Button TrajectAButton;
    public Button TrajectBButton;

    private string _trajectId;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePatientButton.onClick.AddListener(CreateChild);
        BackButton.onClick.AddListener(Back);
        TrajectAButton.GetComponent<Image>().color = Color.white;
        TrajectAButton.onClick.AddListener(OnTrajectAPressed);
        TrajectBButton.GetComponent<Image>().color = Color.white;
        TrajectBButton.onClick.AddListener(OnTrajectBPressed);
    }

    public void OnTrajectAPressed()
    {
        _trajectId = "95967735-0d27-4c36-9818-5b00b77ce5a9";
        TrajectAButton.interactable = false;
        TrajectAButton.GetComponent<Image>().color = Color.blue;
        TrajectBButton.interactable = true;
        TrajectBButton.GetComponent<Image>().color = Color.white;
    }
    
    public void OnTrajectBPressed()
    {
        _trajectId = "15967735-0d27-4c36-9818-5b00b77ce5a9";
        TrajectBButton.interactable = false;
        TrajectBButton.GetComponent<Image>().color = Color.blue;
        TrajectAButton.interactable = true;
        TrajectAButton.GetComponent<Image>().color = Color.white;
    }
    
    private bool Verify()
    {
        if (NameInputField.text.Length <= 1 || string.IsNullOrEmpty(NameInputField.text))
        {
            ErrorText.text = "Naam moet minimaal 1 character lang zijn";
            return false;
        }

        if (string.IsNullOrEmpty(_trajectId))
        {
            ErrorText.text = "Selecteer een traject";
            return false;
        }
        
        ErrorText.text = "";
        return true;
    }

    public async void CreateChild()
    {
        if (!Verify()) return;
        ChildDto child = new ChildDto(_trajectId, "", NameInputField.text, 0);
        Debug.Log("Child JSON: " + JsonUtility.ToJson(child));
        ChildItem childresult = JsonUtility.FromJson<ChildItem>(await ApiClient.PerformApiCall(ApiClient.apiurl + "/child", "POST", JsonUtility.ToJson(child)));
        PlayerPrefs.SetString("SelectedChildName", childresult.Name);
        PlayerPrefs.SetString("SelectedChildId", childresult.Id);
        PlayerPrefs.SetString("SelectedTrajectId", _trajectId);
        
        string userType = PlayerPrefs.GetString("UserType");
        if (userType == "Ouder")
        {
            SceneManager.LoadScene("AfsprakenScene");
        } else if (userType == "Child")
        {
            SceneManager.LoadScene("RoadMapScene");
        } else
        {
            Debug.LogError("UserType not found in PlayerPrefs");
        }
    }
    
    public void Back()
    {
        SceneManager.LoadScene("ChildSelectionScene");
    }
}
