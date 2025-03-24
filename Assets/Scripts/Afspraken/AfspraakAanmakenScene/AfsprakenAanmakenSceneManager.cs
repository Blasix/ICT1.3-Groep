using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using TMPro;

public class AfsprakenAanmakenSceneManager : MonoBehaviour
{
    public TMP_InputField AppointmentNameInputField;
    public TMP_InputField DateInputField;
    public TMP_InputField AttendingDoctorName;

    public TMP_Text TmpTextBannerErrorNaamAfspraak;
    public TMP_Text TmpTextBannerErrorDatumAfspraak;
    public TMP_Text TmpTextBannerErrorNaamBehandelendArts;

    private AppointmentItem appointment;
    private ApiClient _apiClient;
    public void OnappointmentAanmakenButtonClick()
    {
        Debug.Log("appointment aanmaken button clicked");
        appointmentAanmaken();
    }

    public void OnTerugButtonClicked()
    {
        SceneManager.LoadScene("AfsprakenScene");
    }

    public async void appointmentAanmaken()
    {
        string response = await _apiClient.CheckForDuplicateAppointment(AppointmentNameInputField.text);

        if (response == "true")
        {
            Debug.Log("appointment bestaat al");
        }
        else if (response == "false")
        {
            Debug.Log("appointment bestaat nog niet");
            appointment = new AppointmentItem();
            appointment.Id = System.Guid.NewGuid().ToString();
            appointment.AppointmentName = AppointmentNameInputField.text;
            appointment.AppointmentDate = DateInputField.text;
            appointment.NameAttendingDoctor = AttendingDoctorName.text;
            appointment.ChildId = PlayerPrefs.GetString("SelectedChildID");
            appointment.LevelId = PlayerPrefs.GetString("SelectedLevelID");
            appointment.StatusLevel = "NotStarted";
            _apiClient.PostAppointment(appointment);
            SceneManager.LoadScene("AfsprakenScene");
        }
        else
        {
            Debug.LogError("Error: " + response);
        }
    }
}
