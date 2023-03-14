using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoManager.Services.Navigation;
using Windows.Security.Cryptography.Core;

namespace ToDoManager.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public SettingsViewModel(INavigationService navigationService) {
            _navigationService = navigationService;
        }
       
        public ICommand LogoutCommand => new Command(() => LogOutCom());

       private void LogOutCom()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] = "";
            _navigationService.FrameBack();
            _navigationService.NavigateTo<LoginViewModel>();
        }
    }
}
