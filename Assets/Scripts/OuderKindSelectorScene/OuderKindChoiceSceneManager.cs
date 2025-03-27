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
        PlayerPrefs.SetString("UserType", "Ouder");
        SceneManager.LoadScene("ChildSelectionScene");
    }

    public void OnKindButtonPressed()
    {
        Debug.Log("Kind button pressed");
        PlayerPrefs.SetString("UserType", "Child");
        SceneManager.LoadScene("ChildSelectionScene");
    }
}
