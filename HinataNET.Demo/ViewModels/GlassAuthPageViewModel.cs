using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace HinataNET.Demo.ViewModels;

public partial class GlassAuthPageViewModel : ViewModelBase, IPageViewModel
{
    public GlassAuthPageViewModel() => Title = "Glass Auth Form";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _username = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _email = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private bool _isTermsAccepted;

    [ObservableProperty]
    private bool _hasUsernameError;

    [ObservableProperty]
    private string _usernameErrorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasEmailError;

    [ObservableProperty]
    private string _emailErrorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasPasswordError;

    [ObservableProperty]
    private string _passwordErrorMessage = string.Empty;

    [ObservableProperty]
    private bool _hasConfirmPasswordError;

    [ObservableProperty]
    private string _confirmPasswordErrorMessage = string.Empty;

    partial void OnUsernameChanged(string value) => ValidateUsername();
    partial void OnEmailChanged(string value) => ValidateEmail();
    partial void OnPasswordChanged(string value) => ValidatePasswords();
    partial void OnConfirmPasswordChanged(string value) => ValidatePasswords();

    private void ValidateUsername()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            HasUsernameError = false;
            UsernameErrorMessage = string.Empty;
            return;
        }

        if (Username.Length < 3)
        {
            HasUsernameError = true;
            UsernameErrorMessage = "Username must be at least 3 characters";
            return;
        }

        HasUsernameError = false;
        UsernameErrorMessage = string.Empty;
    }

    private void ValidateEmail()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            HasEmailError = false;
            EmailErrorMessage = string.Empty;
            return;
        }

        bool isValid = Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        HasEmailError = !isValid;
        EmailErrorMessage = isValid ? string.Empty : "Invalid email format";
    }

    private void ValidatePasswords()
    {
        HasPasswordError = false;
        PasswordErrorMessage = string.Empty;
        HasConfirmPasswordError = false;
        ConfirmPasswordErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(ConfirmPassword))
            return;

        if (Password.Length > 0 && Password.Length < 6)
        {
            HasPasswordError = true;
            PasswordErrorMessage = "Password must be at least 6 characters";
        }

        if (Password.Length >= 6)
        {
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                HasConfirmPasswordError = true;
                ConfirmPasswordErrorMessage = "Confirm your password";
            }
            else if (Password != ConfirmPassword)
            {
                HasConfirmPasswordError = true;
                ConfirmPasswordErrorMessage = "Passwords do not match";
            }
        }
    }

    private bool CanRegister()
    {
        return !string.IsNullOrWhiteSpace(Username) && !HasUsernameError &&
               !string.IsNullOrWhiteSpace(Email) && !HasEmailError &&
               !string.IsNullOrWhiteSpace(Password) && !HasPasswordError &&
               !string.IsNullOrWhiteSpace(ConfirmPassword) && !HasConfirmPasswordError &&
               IsTermsAccepted;
    }

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private void Register()
    {
        Debug.WriteLine($"[Auth] Registering User: {Username}, Email: {Email}");
    }
}