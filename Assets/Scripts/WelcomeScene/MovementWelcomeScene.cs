using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovementWelcomeScene : MonoBehaviour
{
    public Button Loginbutton;
    public Button Registerbutton;
    public Image LoginbuttonImage;
    public Image RegisterbuttonImage;
    void Start()
    {
        LoginbuttonImage.color = new Color(0f, 0f, 0f, 0.5f);
        RegisterbuttonImage.color = new Color(0f, 0f, 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToLoginScene()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }

    public void MoveToRegisterScene()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("RegisterScene");
    }
}
