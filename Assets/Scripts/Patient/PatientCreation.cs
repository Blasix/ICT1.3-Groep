using Patient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PatientCreation : MonoBehaviour
{
    public TMP_InputField FirstNameInputField;
    public TMP_InputField LastNameInputField;
    
    public Button CreatePatientButton;
    public Button BackButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePatientButton.onClick.AddListener(CreatePatient);
        BackButton.onClick.AddListener(Back);
    }

    public void CreatePatient()
    {
        PatientDto patient = new PatientDto("", "", FirstNameInputField.text, LastNameInputField.text);
        Debug.Log("Patient created: " + patient.vooraam + " " + patient.achternaam);
    }
    
    public void Back()
    {
        // TODO terug naar kind selectie scherm
    }
}
