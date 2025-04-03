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
    public Button TrajectBButton;

    private string _trajectId = "95967735-0d27-4c36-9818-5b00b77ce5a9";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePatientButton.onClick.AddListener(CreateChild);
        BackButton.onClick.AddListener(Back);
        TrajectBButton.GetComponent<Image>().color = Color.white;
        TrajectBButton.onClick.AddListener(OnTrajectBPressed);
    }
    
    public void OnTrajectBPressed()
    {
        string trajectB = "15967735-0d27-4c36-9818-5b00b77ce5a9";
        string trajectA = "95967735-0d27-4c36-9818-5b00b77ce5a9";
        if (_trajectId.Equals(trajectB))
        {
            _trajectId = trajectA;
            TrajectBButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _trajectId = trajectB;
            TrajectBButton.GetComponent<Image>().color = Color.blue;
        }
        Debug.Log(_trajectId);
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
        var result = await ApiClient.PerformApiCall(ApiClient.apiurl + "/child", "POST", JsonUtility.ToJson(child));
        Debug.Log("Result: " + result);
        ChildItem childresult = JsonUtility.FromJson<ChildItem>(result);
        PlayerPrefs.SetString("SelectedChildName", childresult.name);
        PlayerPrefs.SetString("SelectedChildId", childresult.id);
        PlayerPrefs.SetString("SelectedTrajectId", childresult.trajectId);
        
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
