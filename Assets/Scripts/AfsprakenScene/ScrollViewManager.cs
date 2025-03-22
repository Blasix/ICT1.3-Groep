using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject itemPrefab; // Prefab for the list item
    public Transform contentParent; // Content object of the ScrollView
    public AppointmentApi appointmentAPI; // Reference to the API class

    private AppointmentItemManager appointmentManager;

    void Start()
    {
        // Initialize the appointment manager
        appointmentManager = new AppointmentItemManager();

        // Fetch the list of appointments
        var appointments = appointmentManager.GetAppointments();

        AddItems(appointments);
    }

    void AddItems(List<AppointmentItem> appointments)
    {
        // Check if prefab and content parent are assigned
        if (itemPrefab == null || contentParent == null)
        {
            Debug.LogError("Prefab or Content Parent is not assigned!");
            return;
        }

        // Loop through the list of appointments
        foreach (var appointment in appointments)
        {
            // Instantiate the prefab
            GameObject newItem = Instantiate(itemPrefab, contentParent);

            // Set the text values
            TMP_Text nameText = newItem.transform.Find("ButtonBekijkAfspraak/TMP_TextNaamAfspraakVar")?.GetComponent<TMP_Text>();
            TMP_Text dateText = newItem.transform.Find("ButtonBekijkAfspraak/TMP_TextDatumAfspraakVar")?.GetComponent<TMP_Text>();
            TMP_Text doctorText = newItem.transform.Find("ButtonBekijkAfspraak/TMP_TextBehalndelendeArtsVar")?.GetComponent<TMP_Text>();

            if (nameText != null)
            {
                nameText.text = appointment.AppointmentName;
            }
            else
            {
                Debug.LogError("TMP_TextNaamAfspraakVar not found in itemPrefab");
            }

            if (dateText != null)
            {
                dateText.text = appointment.AppointmentDate;
            }
            else
            {
                Debug.LogError("TMP_TextDatumAfspraakVar not found in itemPrefab");
            }

            if (doctorText != null)
            {
                doctorText.text = appointment.attendingDoctor;
            }
            else
            {
                Debug.LogError("TMP_TextBehalndelendeArtsVar not found in itemPrefab");
            }

            // Set up the delete button
            Button deleteButton = newItem.transform.Find("ButtonVerwijderAfspraak")?.GetComponent<Button>();
            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(() => DeleteItem(newItem));
            }
            else
            {
                Debug.LogError("DeleteButton not found in itemPrefab");
            }

            // Make the new item active
            newItem.SetActive(true);
        }
    }

    void DeleteItem(GameObject item)
    {
        Destroy(item);
    }
}
