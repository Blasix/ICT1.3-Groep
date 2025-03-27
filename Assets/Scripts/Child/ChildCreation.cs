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
    
    public Button CreatePatientButton;
    public Button BackButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePatientButton.onClick.AddListener(CreateChild);
        BackButton.onClick.AddListener(Back);
    }

    public async void CreateChild()
    {
        ChildDto child = new ChildDto("15967735-0d27-4c36-9818-5b00b77ce5a9", "", NameInputField.text, 0);
        Debug.Log("Child JSON: " + JsonUtility.ToJson(child));
        ChildItem childresult = JsonUtility.FromJson<ChildItem>(await ApiClient.PerformApiCall(ApiClient.apiurl + "/child", "POST", JsonUtility.ToJson(child)));
        PlayerPrefs.SetString("SelectedChildName", childresult.Name);
        PlayerPrefs.SetString("SelectedChildId", childresult.Id);
        // TODO doorsturen naar app
    }
    
    public void Back()
    {
        SceneManager.LoadScene("ChildSelectionScene");
    }
}
