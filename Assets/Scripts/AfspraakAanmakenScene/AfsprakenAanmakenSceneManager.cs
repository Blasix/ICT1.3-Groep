using UnityEngine;
using UnityEngine.SceneManagement;

public class AfsprakenAanmakenSceneManager : MonoBehaviour
{
    public void OnAfspraakAanmakenButtonClick()
    {
        Debug.Log("Afspraak aanmaken button clicked");
    }

    public void OnTerugButtonClicked()
    {
        SceneManager.LoadScene("AfsprakenScene");
    }
}
