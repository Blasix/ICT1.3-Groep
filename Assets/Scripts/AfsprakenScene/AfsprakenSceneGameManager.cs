using UnityEngine;
using UnityEngine.SceneManagement;

public class AfsprakenSceneGameManager : MonoBehaviour
{
    public void OnAfspraakMakenButtonClick()
    {
        SceneManager.LoadScene("NieuweAfspraakCreationScene");
    }
}
