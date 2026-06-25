using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HinataNET.Controls;

namespace HinataNET.Demo.ViewModels;

public partial class PasswordStrengthMeterPageViewModel : ViewModelBase, IPageViewModel
{
    [ObservableProperty]
    private string _userPassword = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private PasswordStrength _currentStrength = PasswordStrength.None;

    public PasswordStrengthMeterPageViewModel() => Title = "Password Strength Meter";

    private bool CanSubmit()
    {
        return CurrentStrength == PasswordStrength.Good || CurrentStrength == PasswordStrength.Strong;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private void Submit()
    {
        // Action upon successful confirmation
    }
}