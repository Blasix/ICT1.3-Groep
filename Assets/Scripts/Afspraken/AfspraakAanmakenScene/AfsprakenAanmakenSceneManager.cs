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
    public TMP_InputField AttendingDoctorName;

    public TMP_Text TmpTextBannerGeneralError;
    public TMP_Text TmpTextBannerErrorNaamAfspraak;
    public TMP_Text TmpTextBannerErrorDatumAfspraak;
    public TMP_Text TmpTextBannerErrorNaamBehandelendArts;

    private AppointmentItem _appointment;
    private ApiClient _apiClient;
    private InputValidator _inputValidator;

    private string _enteredAppointmentName;
    private string _enteredDate;
    private string _enteredAttendingDoctorName;

    void Start()
    {
        _appointment = new AppointmentItem();
        _apiClient = new ApiClient();
        _inputValidator = new InputValidator();
        TmpTextBannerGeneralError.text = "";
        TmpTextBannerErrorNaamAfspraak.text = "";
        TmpTextBannerErrorDatumAfspraak.text = "";
        TmpTextBannerErrorNaamBehandelendArts.text = "";
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
        _enteredAppointmentName = _appointmentNameInputField.text;
        _enteredDate = DateInputField.text;
        _enteredAttendingDoctorName = AttendingDoctorName.text;
        var (isValidName, potentialErrorName) = _inputValidator.ValidateAppointmentName(_enteredAppointmentName);
        var (isValidDate, potentialErrorDate) = _inputValidator.ValidateDate(_enteredDate);
        var (isValidAttendingDoctorName, potentialErrorAttendingDoctorName) = _inputValidator.ValidateAttendingDoctorName(_enteredAttendingDoctorName);

        if (!isValidName || !isValidDate || !isValidAttendingDoctorName)
        {
            TmpTextBannerErrorNaamAfspraak.text = potentialErrorName;
            TmpTextBannerErrorDatumAfspraak.text = potentialErrorDate;
            TmpTextBannerErrorNaamBehandelendArts.text = potentialErrorAttendingDoctorName;
            return;
        }

        string response = await _apiClient.CheckForDuplicateAppointment(_enteredAppointmentName);

        if (response == "true")
        {
            Debug.Log("Appointment allready exists");
        }

        Debug.Log("Appointment does not exist yet creating appointment");
        _appointment = new AppointmentItem();
        _appointment.Id = System.Guid.NewGuid().ToString();
        _appointment.AppointmentName = _enteredAppointmentName;
        _appointment.AppointmentDate = _enteredDate;
        _appointment.NameAttendingDoctor = _enteredAttendingDoctorName;
        _appointment.ChildId = PlayerPrefs.GetString("SelectedChildID");
        _appointment.LevelId = PlayerPrefs.GetString("SelectedLevelID");
        _appointment.StatusLevel = "NotStarted";
        _apiClient.PostAppointment(_appointment);
        SceneManager.LoadScene("AfsprakenScene");
    }
    
}
