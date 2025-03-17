using UnityEngine;
using UnityEngine.SceneManagement;
public class RegisterSceneManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    { }

    public void OnClickLoginButton()
    {
        Debug.Log("Loading login scene");
        SceneManager.LoadScene("LoginScene");
    }
}