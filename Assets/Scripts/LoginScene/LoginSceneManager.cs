using UnityEngine;
using UnityEngine.SceneManagement;
public class LoginSceneManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {}

    public void OnClickRegisterButton()
    {
        Debug.Log("Loading login scene");
        SceneManager.LoadScene("RegisterScene");
    }
}
