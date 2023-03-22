using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoManager.Services.Navigation;

namespace ToDoManager.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
       public ShellViewModel(INavigationService navigationService)  
        {
            _navigationService = navigationService;
            LoadMenuItems();
        }
        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems => _menuItems;

        private ObservableCollection<MenuItem> _footerMenuItems;
        public ObservableCollection<MenuItem> FooterMenuItems => _footerMenuItems;

        public void LoadMenuItems()
        {
            _menuItems = new ObservableCollection<MenuItem>();
            _menuItems.Add(new MenuItem { Name = "To Do List", Icon = "\uE71D", ViewModelType = typeof(TasksViewModel) });
            _menuItems.Add(new MenuItem { Name = "Time Manager", Icon = "\uE823", ViewModelType = typeof(TimeMangerViewModel) });

            _footerMenuItems = new ObservableCollection<MenuItem>();
            _footerMenuItems.Add(new MenuItem { Name = "LogOut", Icon = "\uE72B", ViewModelType = null });
        }

        public ICommand SelectedPageChangedCommand => new Command<MenuItem>((value) => NavigateToMenuItem(value));
        private void NavigateToMenuItem(MenuItem item)
        {
            Type page = item.ViewModelType;
            _navigationService.NavigateTo(page);
        }

        public void Logout()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] = "";
            _navigationService.FrameBack();
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Type ViewModelType { get; set; }
    }
}
