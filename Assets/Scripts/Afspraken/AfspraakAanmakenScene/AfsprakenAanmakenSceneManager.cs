using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using NUnit.Framework;
using TMPro;

public class AfsprakenAanmakenSceneManager : MonoBehaviour
{
    public TMP_InputField _appointmentNameInputField;
    public TMP_InputField DateInputField;

    public TMP_Text TmpTextBannerGeneralError;
    public TMP_Text TmpTextBannerErrorNaamAfspraak;
    public TMP_Text TmpTextBannerErrorDatumAfspraak;

    private AppointmentItem _appointment;
    private ApiClient _apiClient;
    private InputValidator _inputValidator;

    private string _enteredAppointmentName;
    private string _enteredDate;
    private string _enteredAttendingDoctorName;
    private string childName;

    void Start()
    {
        _apiClient = new ApiClient();
        _inputValidator = new InputValidator();
        TmpTextBannerGeneralError.text = "";
        TmpTextBannerErrorNaamAfspraak.text = "";
        TmpTextBannerErrorDatumAfspraak.text = "";
        PlayerPrefs.SetString("SelectedLevelId", "2b3098fd-f3c3-4321-aaf7-8f74f070b8a5");
        childName = PlayerPrefs.GetString("SelectedChildName");
    }

    public void OnAppointmentAanmakenButtonClick()
    {
        Debug.Log("_appointment aanmaken button clicked");
        _appointmentAanmaken();
    }

    public void OnTerugButtonClicked()
    {
        SceneManager.LoadScene("AfsprakenScene");
    }

    public async void _appointmentAanmaken()
    {
        _enteredAppointmentName = _appointmentNameInputField?.text;
        _enteredDate = DateInputField?.text;

        var (isValidName, potentialErrorName) = _inputValidator.ValidateAppointmentName(_enteredAppointmentName);
        var (isValidDate, potentialErrorDate) = _inputValidator.ValidateDate(_enteredDate);

        if (!isValidName || !isValidDate)
        {
            TmpTextBannerErrorNaamAfspraak.text = potentialErrorName;
            TmpTextBannerErrorDatumAfspraak.text = potentialErrorDate;
            return;
        }

        bool response = await _apiClient.CheckForDuplicateAppointment(childName, _enteredAppointmentName);

        if (response)
        {
            Debug.Log("Appointment already exists");
            TmpTextBannerGeneralError.text = "Appointment already exists";
            return;
        }

        Debug.Log("Appointment does not exist yet, creating appointment");
        _appointment = new AppointmentItem();
        _appointment.id = Guid.NewGuid().ToString();
        _appointment.appointmentName = _enteredAppointmentName;
        _appointment.date = _enteredDate;
        _appointment.childId = PlayerPrefs.GetString("SelectedChildId");
        _appointment.levelId = PlayerPrefs.GetString("SelectedLevelId");
        _appointment.statusLevel = "completed";
        _appointment.LevelStep = 0;
        

        Debug.Log($"Appointment Details: id={_appointment.id}, appointmentName={_appointment.appointmentName}, date={_appointment.date}, childId={_appointment.childId}, levelId={_appointment.levelId}, statusLevel={_appointment.statusLevel}, LevelStep: {_appointment.LevelStep}");
        await _apiClient.PostAppointment(_appointment);
        Debug.Log("Done loading scene");
        SceneManager.LoadScene("AfsprakenScene");
    }
}
