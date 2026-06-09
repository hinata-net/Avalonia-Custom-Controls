using CommunityToolkit.Mvvm.ComponentModel;

namespace HinataNET.Demo.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;
    }
}
