using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Assets.Scripts.Objects;

public class AppointmentsSceneManager : MonoBehaviour
{
    public TMP_InputField AppointmentInputField;
    public TMP_InputField DateInputField;
    public GameObject[] appointmentItems;

    [System.Serializable]
    private class AppointmentListWrapper
    {
        public List<Appointment> appointments;
    }

    public void OnClickSaveButton()
    {
        List<Appointment> appointments = new List<Appointment>();

        for (int i = 0; i < appointmentItems.Length; i++)
        {
            TMP_Text[] texts = appointmentItems[i].GetComponentsInChildren<TMP_Text>();
            Appointment appointment = new Appointment();
            appointment.appointmentName = texts[0].text;
            appointment.date = texts[1].text;
            appointments.Add(appointment);
        }

        AppointmentListWrapper wrapper = new AppointmentListWrapper();
        wrapper.appointments = appointments;
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("Appointments", json);
        PlayerPrefs.Save();
    }
}