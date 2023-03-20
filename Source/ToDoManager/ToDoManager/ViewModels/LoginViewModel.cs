using Microsoft.UI.Xaml.Media;
using System.Windows.Input;
using ToDoManager.HelpClasses.Verify;
using ToDoManager.Services.Authentication;
using ToDoManager.Services.Navigation;

namespace ToDoManager.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel(
            INavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        private Brush _statusColor;
        public Brush StatusColor
        {
            get => _statusColor;
            set { _statusColor = value; OnPropertyChanged(nameof(StatusColor)); }
        }

        public ICommand ToRegisterPageCommand => new Command(() => _navigationService.NavigateTo(typeof(RegisterViewModel)));

        public ICommand Sign_In => new Command(() => SignIn());

        private async void SignIn()
        {
            Status = Verfy.FieldsIsValidSignIn(_email, _password);
            if (_status == "")
            {
                Status = await _authenticationService.SignInAsync(_email, _password);
                if(_status == "")
                {
                    _navigationService.NavigateTo(typeof(ShellViewModel));
                }
            }  
        }  
    }
}
