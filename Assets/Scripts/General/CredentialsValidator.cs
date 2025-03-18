using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using System.Linq;


public class CredentialsValidator
{
    private string _enteredEmail;
    private string _enteredPassword;

    public (bool, string) ValidateEmail(string Email)
    {
        _enteredEmail = Email;

        if (string.IsNullOrEmpty(_enteredEmail))
        {
            return (false, "Please enter a email address");
        }
        else if (_enteredEmail.Length < 5)
        {
            return (false, "Email must be at least 5 characters");
        }
        else if (!IsValidEmail(_enteredEmail))
        {
            return (false, "Invalid email format");
        }
        else
        {
            return (true, "");
        }

    }

    public (bool, string) ValidatePassword(string Password)
    {
        _enteredPassword = Password;

        if (string.IsNullOrEmpty(_enteredPassword))
        {
            return (false, "Please enter a password");
        }
        else if (_enteredPassword.Length < 10)
        {
            return (false, "Password length should be longer than 10 characters");
        }
        else if (_enteredPassword.Any(char.IsDigit) == false)
        {
            return (false, "Password should contain at least one digit");
        }
        else if (_enteredPassword.Any(char.IsUpper) == false)
        {
            return (false, "Password should contain at least one uppercase letter");
        }
        else if (_enteredPassword.Any(char.IsLower) == false)
        {
            return (false, "Password should contain at least one lowercase letter");
        }
        else if (_enteredPassword.Any(char.IsLetterOrDigit) == false)
        {
            return (false, "Password should contain at least one special character");
        }
        else
        {
            return (true, "");
        }
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}
