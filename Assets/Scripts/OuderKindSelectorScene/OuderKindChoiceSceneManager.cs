using UnityEngine;
using UnityEngine.SceneManagement;

public class OuderKindChoiceSceneManager : MonoBehaviour
{
    public void OnLogoutButtonPressed()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("WelcomeScene");
    }

    public void OnOuderButtonPressed()
    {
        Debug.Log("Ouder button pressed");
        //Doorverwijzing naar Ouder omgeving
    }

    public void OnKindButtonPressed()
    {
        Debug.Log("Kind button pressed");
        //Doorverwijzing naar Kind omgeving
    }
}
