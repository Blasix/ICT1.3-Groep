using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ApiClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RoadMapScene : MonoBehaviour
{
    public GameObject nodePrefab;  // Assign a prefab with an Image and a circular shape
    public Transform roadmapContainer;

    private List<Appointment> appointments; // Store the appointments globally so we can use them on click.

    void Start()
    {
        GetAppointments();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void GetAppointments()
    {
        ApiClient apiClient = new ApiClient();
        appointments = await apiClient.GetAllChildAppointment();
        if (appointments != null)
        {
            GenerateRoadMap(appointments);
        }
        else
        {
            Debug.LogError("Failed to retrieve appointments.");
        }
    }

    public void GenerateRoadMap(List<Appointment> appointments)
    {
        float startX = -((appointments.Count - 1) * 100f); // Reduced the starting X distance for closer nodes
        float amplitudeY = 250f; // The maximum height of the zigzag movement
        float frequencyY = 1f; // Controls the "tightness" of the zigzag (higher value = tighter)
        float spacingX = 100f; // Adjusted spacing for X positions to spread out the nodes more

        for (int i = 0; i < appointments.Count; i++)
        {
            GameObject node = Instantiate(nodePrefab, roadmapContainer);
            Image nodeImage = node.GetComponent<Image>();

            // Apply smooth, eased X position (start closer, end later)
            float x = startX + i * spacingX;

            // Use sine wave to create smooth, flowing oscillation for the Y position
            float y = Mathf.Sin(i * frequencyY) * amplitudeY;

            // Apply eased X position to ensure smooth transition with later end
            float easedX = Mathf.SmoothStep(startX, startX + (appointments.Count - 1) * spacingX * 2f, (float)i / (appointments.Count - 1));

            node.transform.localPosition = new Vector3(easedX, y, 0f);

            // Add a pointer click listener for the current node to save data and switch scenes
            int index = i; // Local copy of the index to avoid closure issues in the listener
            node.AddComponent<PointerClickHandler>().Initialize(() => OnNodeClick(appointments[index]));
        }
    }

    // Called when a node is clicked
    public void OnNodeClick(Appointment selectedAppointment)
    {
        Debug.Log("clicked");
        // Save the selected appointment details in PlayerPrefs
        PlayerPrefs.SetString("SelectedAppointmentId", selectedAppointment.Id.ToString());
        PlayerPrefs.SetString("SelectedAppointmentName", selectedAppointment.Name);
        PlayerPrefs.SetString("SelectedAppointmentDate", selectedAppointment.Date.ToString());
        PlayerPrefs.SetString("SelectedAppointmentUrl", selectedAppointment.Url);

        // Load the AvatarSelectorScene
        SceneManager.LoadScene("AvatarSelectorScene");
    }
}

public class PointerClickHandler : MonoBehaviour, IPointerClickHandler
{
    private System.Action onClick;

    public void Initialize(System.Action onClickAction)
    {
        onClick = onClickAction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}

