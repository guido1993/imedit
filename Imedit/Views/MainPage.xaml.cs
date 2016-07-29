using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Imedit.ViewModels;

namespace Imedit.Views
{
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel { get { return this.DataContext as MainPageViewModel; } }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.Init();
        }
    }
}