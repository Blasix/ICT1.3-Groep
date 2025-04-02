using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RegisterSceneManager : MonoBehaviour
{
    public TMP_InputField EmailInputField;
    public TMP_InputField PasswordInputField;

    public TMP_Text TmpTextBannerErrorGeneral; // Algemene error veldt
    public TMP_Text TmpTextBannerErrorUsername; // Error veldt voor username
    public TMP_Text TmpTextBannerErrorPassword; // Error veldt voor password

    private string _enteredEmail; //De ingevoerde email
    private string _enteredPassword; //Het ingevoerde password

    private ApiClient _apiClient;
    private InputValidator _inputValidator; //CredentialsValidator voor het nakijken of het valide credentails zijn

    private void Start()
    {
        _inputValidator = gameObject.AddComponent<InputValidator>();
        _apiClient = gameObject.AddComponent<ApiClient>();
    }

    public void OnClickRegisterButton()
    {
        /*
         * vraagt de ingevoerde email en password op en valideert deze met input validator.
         * deze klasse geeft een bool terug of de email en password valide zijn en als deze
         * niet valide zijn geeft deze een negatieve bool en een string terug waarin staat wat er fout is.
         */
        _enteredEmail = EmailInputField.text;
        _enteredPassword = PasswordInputField.text;
        var (isValidEmail, ifApplicableEmailError) = _inputValidator.ValidateEmail(_enteredEmail);
        var (isValidPassword, ifApplicablePasswordError) = _inputValidator.ValidatePassword(_enteredPassword);
        if (isValidEmail && isValidPassword)
        {
            _apiClient.Register(_enteredEmail, _enteredPassword);
            TmpTextBannerErrorGeneral.text = "Email or password wrong!";
        }
        else
        {
            TmpTextBannerErrorUsername.text = ifApplicableEmailError;
            TmpTextBannerErrorPassword.text = ifApplicablePasswordError;
        }
    }

    public void OnClickLoginButton() //Als het kleine switchknopje wordt ingedrukt om naar login te gaan
    {
        Debug.Log("Loading login scene");
        SceneManager.LoadScene("LoginScene");
    }
}