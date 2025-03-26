using UnityEngine;
[System.Serializable]
public class AppointmentItem : MonoBehaviour
{
    public string id { get; set; }
    public string appointmentName { get; set; }
    public string date { get; set; }
    public string childId { get; set; }
    public string levelId { get; set; }
    public string statusLevel { get; set; }
    public int LevelStep { get; set; }
}

