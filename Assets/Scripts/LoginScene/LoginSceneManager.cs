using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
public class LoginSceneManager : MonoBehaviour
{
    public TMP_InputField EmailInputField;
    public TMP_InputField PasswordInputField;

    public TMP_Text TmpTextBannerErrorGeneral; // Algemene error veldt
    public TMP_Text TmpTextBannerErrorUsername; // Error veldt voor username
    public TMP_Text TmpTextBannerErrorPassword; // Error veldt voor password

    private ApiClient _apiClient;
    private CredentialsValidator _credentialsValidator; //CredentialsValidator voor het nakijken of het valide credentails zijn

    private void Start()
    {
        _credentialsValidator = new CredentialsValidator();
        _apiClient = new ApiClient();
    }
    
    private string _enteredEmail;
    private string _enteredPassword;

    void Update()
    {}

    public void OnLoginButtonPressed()
    {
        /*
         * vraagt de ingevoerde email en password op en valideert deze met credential validator.
         * deze klasse geeft een bool terug of de email en password valide zijn en als deze
         * niet valide zijn geeft deze een negatieve bool en een string terug waarin staat wat er fout is.
         */
        _enteredEmail = EmailInputField.text;
        _enteredPassword = PasswordInputField.text;
        var (isValidEmail, ifApplicableEmailError) = _credentialsValidator.ValidateEmail(_enteredEmail); 
        var (isValidPassword, ifApplicablePasswordError) = _credentialsValidator.ValidatePassword(_enteredPassword);
        if (isValidEmail && isValidPassword)
        {
            _apiClient.Login(_enteredEmail, _enteredPassword);
            TmpTextBannerErrorGeneral.text = "Email or password wrong!";
        }
        else
        {
            TmpTextBannerErrorUsername.text = ifApplicableEmailError;
            TmpTextBannerErrorPassword.text = ifApplicablePasswordError;
        }
    }

    public void OnClickRegisterButton()
    {
        Debug.Log("Loading login scene");
        SceneManager.LoadScene("RegisterScene");
    }
}
