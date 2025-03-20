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
        ChildDto child = new ChildDto( "", "", NameInputField.text, 0);
        Debug.Log("Child JSON: " + JsonUtility.ToJson(child));
        await ApiClient.PerformApiCall(ApiClient.apiurl + "/child", "POST", JsonUtility.ToJson(child));
        // TODO doorsturen naar app
    }
    
    public void Back()
    {
        // TODO terug naar kind selectie scherm
    }
}
