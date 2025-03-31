using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ApiClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class RoadMapScript : MonoBehaviour
{
    public Transform roadmapContainerA;
    public Transform roadmapContainerB;
    private string ChildTraject = "95967735-0d27-4c36-9818-5b00b77ce5a9";
    private string ChildId;
    private List<AppointmentItem> appointments;

    void Start()
    {
         if (!string.IsNullOrEmpty(ChildTraject))
        {
            roadmapContainerA.gameObject.SetActive(true);

        }
        else
        {
            roadmapContainerA.gameObject.SetActive(false);
            roadmapContainerB.gameObject.SetActive(false);
        }
        GetAppointments();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void GetAppointments()
    {
        ApiClient apiClient = new ApiClient();
        string childName = "Bob";
        appointments = await apiClient.GetAppointments(childName);
        if (appointments != null)
        {
            setApointmentsToItems(appointments);
        }
        else
        {
            Debug.LogError("Failed to retrieve appointments.");
        }
    }

    public void ClickedRoadItem(Image ClickedItem)
    {
        string Step = ClickedItem.name;

        switch (Step)
        {
            case "Step-1":
                PlayerPrefs.SetInt("step", 1);
                SetAppointmentDetails(1);
                break;
            case "Step-2":
                PlayerPrefs.SetInt("step", 2);
                SetAppointmentDetails(2);
                break;
            case "Step-3":
                PlayerPrefs.SetInt("step", 3);
                SetAppointmentDetails(3);
                break;
            case "Step-4":
                PlayerPrefs.SetInt("step", 4);
                SetAppointmentDetails(4);
                break;
            case "Step-5":
                PlayerPrefs.SetInt("step", 5);
                SetAppointmentDetails(5);
                break;
            case "Step-6":
                PlayerPrefs.SetInt("step", 6);
                SetAppointmentDetails(6);
                break;
            case "Step-7":
                PlayerPrefs.SetInt("step", 7);
                SetAppointmentDetails(7);
                break;
            case "Step-8":
                PlayerPrefs.SetInt("step", 8);
                SetAppointmentDetails(8);
                break;
            case "Step-9":
                PlayerPrefs.SetInt("step", 9);
                SetAppointmentDetails(9);
                break;
            default:
                Debug.Log("No matching step found.");
                break;
        }
        PlayerPrefs.Save();
        Debug.Log("Step: " + PlayerPrefs.GetInt("step"));
        SceneManager.LoadScene("WelcomeScene");
    }

    private void SetAppointmentDetails(int step)
    {
        foreach (var appointment in appointments)
        {
            if (appointment.LevelStep == step)
            {
                PlayerPrefs.SetInt("appointment_step", step);
                PlayerPrefs.SetString("appointment_name", appointment.appointmentName);
                PlayerPrefs.SetString("appointment_date", appointment.date); // Save the date as a string
                PlayerPrefs.SetString($"Step-{step}-Date", appointment.date); // Set the date with the key format Step-1-Date
                PlayerPrefs.Save();
                break;
            }
        }
    }

    public void setApointmentsToItems(List<AppointmentItem> appointments)
    {
        Transform activeContainer = null;
            activeContainer = roadmapContainerA;
        

        foreach (Transform item in activeContainer)
        {
            foreach (var appointment in appointments)
            {
                if (item.name == $"Step-{appointment.LevelStep}-Date")
                {
                    // Set appointment details to the item
                    // Assuming the item has a TextMeshProUGUI component to display the appointment date
                    TextMeshProUGUI itemText = item.GetComponentInChildren<TextMeshProUGUI>();
                    if (itemText != null)
                    {
                        if (DateTime.TryParseExact(appointment.date, "MM/dd/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime appointmentDate))
                        {
                            TimeSpan timeUntilAppointment = appointmentDate - DateTime.Now;
                            int daysUntilAppointment = (int)timeUntilAppointment.TotalDays;
                            if (daysUntilAppointment > 1)
                            {
                                itemText.text = $"Nog {daysUntilAppointment} dagen tot de afspraak";
                            }
                            else if (daysUntilAppointment == 0)
                            {
                                itemText.text = "Vandaag is de afspraak";
                            }
                            else
                            {
                                itemText.text = "de afspraak is al geweest";
                            }
                        }
                        else
                        {
                            itemText.text = "Ongeldige datum";
                        }
                    }
                }
                if(item.name == $"Step-{appointment.LevelStep}")
                {
                    Image image = item.GetComponentInChildren<Image>();
                    switch(appointment.statusLevel)
                    {
                        case "completed":
                            image.color = Color.green;
                            break;
                        case "doing":
                            image.color = Color.blue;
                            break;
                        case "incompleted":
                            image.color = Color.red;
                            break;
                        default:
                            image.color = Color.clear;
                            break;
                    }
                }
            }
        }
    }
}



