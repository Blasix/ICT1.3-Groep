using UnityEngine;

[System.Serializable]
public class AppointmentItem : MonoBehaviour
{
    public string AppointmentName; // Name of the appointment
    public string AppointmentDate; // Date of the appointment
    public string attendingDoctor; // Attending doctor
    public string guid; // Unique identifier
    public string childId; // Child ID
}