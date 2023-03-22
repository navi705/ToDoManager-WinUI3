using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Windows.Input;
using ToDoManager.HelpClasses.Verify;
using ToDoManager.Services.Authentication;
using ToDoManager.Services.Navigation;

namespace ToDoManager.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        public RegisterViewModel(INavigationService navigationService,IAuthenticationService authenticationService )
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(nameof(ConfirmPassword)); }
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

        public ICommand ToLoginPageCommand => new Command(() => _navigationService.NavigateTo(typeof(LoginViewModel)));

        public ICommand Sign_Up => new Command(() => SignUp());

        private async void SignUp()
        {
            Status = Verfy.FieldsIsValid(_email, _password, _confirmPassword);
            StatusColor = new SolidColorBrush(Colors.Red);
            if (Status == "")
            { 
                Status = await _authenticationService.SignUpAsync(_email,_password);
                if(Status == "Successfully")
                {
                    StatusColor = new SolidColorBrush(Colors.Green);
                }
            }
        }
    }
}
