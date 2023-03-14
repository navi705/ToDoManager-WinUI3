using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoManager.IoC;
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

        public void LoadMenuItems()
        {
            _menuItems = new ObservableCollection<MenuItem>();

            _menuItems.Add(new MenuItem { Name = "To Do List", Icon = "\uE71D", ViewModelType = typeof(TasksViewModel) });
            _menuItems.Add(new MenuItem { Name = "Time Manager", Icon = "\uE823", ViewModelType = typeof(TimeMangerViewModel) });
            //_menuItems.Add(new MenuItem { Name = "Search", Icon = "\uE721", ViewModelType = nameof(SearchViewModel) });
        }

        public ICommand SelectedPageChangedCommand => new Command<MenuItem>((value) => NavigateToMenuItem(value));
        private void NavigateToMenuItem(MenuItem item)
        {
            Type page = item.ViewModelType;
            _navigationService.NavigateTo(page);
        }

    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Type ViewModelType { get; set; }
    }

}
