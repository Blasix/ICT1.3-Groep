using TMPro;
using UnityEngine;

public class CountdownSceneManager : MonoBehaviour
{
    public TMP_Text TmpTextTitle; // "Nog maar: "
    [SerializeField] public TMP_Text TmpTextCountdown; // Weergeeft D/M/S tot volgende afspraak
    public TMP_Text TmpTextAppointment; // "Tot de operatie" (of andere afspraak)

    // TODO: remainingTime koppelen met backend
    [SerializeField] float remainingTime; // Tijd tussen nu en volgende afspraak

    void Update()
    {
        // TODO: notificatie functionaliteit toevoegen (remainingTIme x = sendNotifiaction)
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; // Iedere frame gaat er x af van remaingtime
        }
        else if (remainingTime < 0) 
        {
            remainingTime = 0; // Voorkomt dat timer negatief gaat
        }
        
        TmpTextCountdown.text = FormatCountdown(); // Zet tijd om naar string
    }

    private string FormatCountdown()
    {
        string countdownText = "";

        int days = Mathf.FloorToInt(remainingTime / 86400);
        int hours = Mathf.FloorToInt((remainingTime % 86400) / 3600);
        int minutes = Mathf.FloorToInt((remainingTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        if (days > 0)
        {
            if (minutes > 1)
            {
                countdownText += string.Format($"{days} Dagen\n");
            }
            else
            {
                countdownText += string.Format($"{days} Dag\n");
            }
        }

        if (hours > 0 || days > 0)
        {
            countdownText += string.Format($"{hours} Uur\n");
        }

        if (minutes > 0 || hours > 0 || days > 0)
        {
            if (minutes > 1)
            {
                countdownText += string.Format($"{minutes} Minuten\n");
            }
            else
            {
                countdownText += string.Format($"{minutes} Minuut\n");
            }
        }

        countdownText += string.Format($"{seconds} Seconden");

        return countdownText;
    }
}
