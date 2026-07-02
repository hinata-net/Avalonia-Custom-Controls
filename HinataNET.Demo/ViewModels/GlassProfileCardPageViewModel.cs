using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace HinataNET.Demo.ViewModels;

public partial class GlassProfileCardPageViewModel : ViewModelBase, IPageViewModel
{
    public GlassProfileCardPageViewModel() => Title = "Glass Profile Card";

    [ObservableProperty]
    private bool _isFollowing;

    [ObservableProperty]
    private bool _isMessageSent;

    [RelayCommand]
    private void ToggleFollow()
    {
        IsFollowing = !IsFollowing;
    }

    [RelayCommand]
    private async Task SendMessageAsync()
    {
        if (IsMessageSent) return;

        IsMessageSent = true;
        await Task.Delay(1500);
        IsMessageSent = false;
    }
}