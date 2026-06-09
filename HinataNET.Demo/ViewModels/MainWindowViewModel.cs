using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HinataNET.Demo.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<IPageViewModel> Pages { get; } = new()
        {
            new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),new GlassAuthPageViewModel(),
        };

        [ObservableProperty]
        private IPageViewModel _currentPage;

        public MainWindowViewModel()
        {
            CurrentPage = Pages[0];
        }
    }
}
