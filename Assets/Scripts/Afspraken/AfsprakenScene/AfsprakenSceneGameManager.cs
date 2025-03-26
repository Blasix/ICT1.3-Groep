using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfsprakenSceneGameManager : MonoBehaviour
{
    public void OnAfspraakMakenButtonClick()
    {
        SceneManager.LoadScene("NieuweAfspraakCreationScene");
    }

    public GameObject itemPrefab; // Prefab for the list item
    public Transform contentParent; // Content object of the ScrollView
    private List<AppointmentItem> _appointmentsList;
    private ApiClient _apiClient;

    public string childName;

    void Start()
    {
        _apiClient = new ApiClient();
        PlayerPrefs.SetString("SelectedChildName", "Bob");
        childName = PlayerPrefs.GetString("SelectedChildName");
        LoadAppointments();
    }

    private async void LoadAppointments()
    {
        _appointmentsList = await GetAppointments();
        AddItems(_appointmentsList);
    }

    private async Task<List<AppointmentItem>> GetAppointments()
    {
        _appointmentsList = await _apiClient.GetAppointments(childName);
        return _appointmentsList;
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

            if (nameText != null)
            {
                nameText.text = appointment.appointmentName;
            }
            else
            {
                Debug.LogError("TMP_TextNaamAfspraakVar not found in itemPrefab");
            }

            if (dateText != null)
            {
                dateText.text = appointment.date;
            }
            else
            {
                Debug.LogError("TMP_TextDatumAfspraakVar not found in itemPrefab");
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

    private async void SendDeleteRequest(string childName, string AppointmentName)
    {
        Debug.Log($"Deleting appointment: {AppointmentName}");
        await _apiClient.DeleteAppointment(childName, AppointmentName);
    }

    public void DeleteItem(GameObject item)
    {
        // Find the text components
        TMP_Text nameText = item.transform.Find("ButtonBekijkAfspraak/TMP_TextNaamAfspraakVar")?.GetComponent<TMP_Text>();

        // Retrieve the text values
        string appointmentName = nameText != null ? nameText.text : "Unknown";

        // Log the text values (or use them as needed)
        SendDeleteRequest(childName, appointmentName);
        Destroy(item);
    }
}
