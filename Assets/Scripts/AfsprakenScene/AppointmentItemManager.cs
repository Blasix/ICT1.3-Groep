using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppointmentItemManager : MonoBehaviour
{
    public List<AppointmentItem> GetAppointments()
    {
        // Simulate a list of appointments (replace with actual data fetching logic)
        return new List<AppointmentItem>
        {
            new AppointmentItem { AppointmentName = "Checkup", AppointmentDate = "2023-10-01", attendingDoctor = "Dr. Smith", guid = "1", childId = "101" },
            new AppointmentItem { AppointmentName = "Vaccination", AppointmentDate = "2023-10-05", attendingDoctor = "Dr. Johnson", guid = "2", childId = "102" },
            new AppointmentItem { AppointmentName = "Follow-up", AppointmentDate = "2023-10-10", attendingDoctor = "Dr. Lee", guid = "3", childId = "103" }
        };
    }
}