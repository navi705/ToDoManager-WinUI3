using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ToDoManager.ViewModels;
using ToDoManager.IoC;

namespace ToDoManager.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }
        
        public LoginViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = DIHelper.Resolve<LoginViewModel>();
        }
    }
}
