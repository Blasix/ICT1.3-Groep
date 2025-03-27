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
        ChildTraject = PlayerPrefs.GetString("ChildTraject");
        if (!string.IsNullOrEmpty(ChildTraject))
        {
            switch (ChildTraject)
            {
                case "95967735-0d27-4c36-9818-5b00b77ce5a9":
                    roadmapContainerA.gameObject.SetActive(true);
                    roadmapContainerB.gameObject.SetActive(false);
                    break;
                case "B":
                    roadmapContainerA.gameObject.SetActive(false);
                    roadmapContainerB.gameObject.SetActive(true);
                    break;
                default:
                    roadmapContainerA.gameObject.SetActive(false);
                    roadmapContainerB.gameObject.SetActive(false);
                    break;
            }
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
        string childName = "name";
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
        SceneManager.LoadScene("Welcome");
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

        if (roadmapContainerA.gameObject.activeSelf)
        {
            activeContainer = roadmapContainerA;
        }
        else if (roadmapContainerB.gameObject.activeSelf)
        {
            activeContainer = roadmapContainerB;
        }
        else
        {
            Debug.LogError("No active roadmap container found.");
            return;
        }

        foreach (Transform item in activeContainer)
        {
            foreach (var appointment in appointments)
            {
                if (item.name == $"Step-{appointment.LevelStep}")
                {
                    // Set appointment details to the item
                    // Assuming the item has a TextMeshProUGUI component to display the appointment date
                    TextMeshProUGUI itemText = item.GetComponentInChildren<TextMeshProUGUI>();
                    if (itemText != null)
                    {
                        if (DateTime.TryParse(appointment.date, out DateTime appointmentDate))
                        {
                            TimeSpan timeUntilAppointment = appointmentDate - DateTime.Now;
                            int daysUntilAppointment = (int)timeUntilAppointment.TotalDays;
                            itemText.text = $"Nog {daysUntilAppointment} dagen tot de afspraak";
                        }
                        else
                        {
                            itemText.text = "Ongeldige datum";
                        }
                    }
                }
            }
        }
    }
}



