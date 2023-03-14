using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoManager.Models;
using ToDoManager.Services.Navigation;
using ToDoManager.Services.Tasks;

namespace ToDoManager.ViewModels
{
    public class AddOrEditTasksViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITasksService _tasksService;
        public AddOrEditTasksViewModel(INavigationService navigationService,ITasksService tasksService)
        {
            _navigationService= navigationService;
            _tasksService = tasksService;
        }

        public ToDoTask Task { get; set; }
        public string TitleTask { get; set; }
        // maybe add converter instead of property
        public DateTimeOffset Date {

            get
            {
                return DateTimeOffset.Parse(Task.Date);
            }
            set
            {
                Task.Date = value.Date.ToShortDateString();
                OnPropertyChanged(nameof(Date));
               
            }

        }

        public TimeSpan Time
        {
            get
            {
                return TimeSpan.Parse(Task.Time);
            }
            set
            {
                Task.Time = value.ToString(@"hh\:mm");
               // OnPropertyChanged(nameof(Time)); broken

            }
        }

        public string AutoSkip
        {
            get
            {
                if (Task.Auto_fail)
                    return "Yes";
                return "No";

            }
            set
            {
                if (value == "Yes")
                    Task.Auto_fail = true;
                else
                    Task.Auto_fail = false;

            }
        }

        public ICommand ToTaskPage => new Command(() => _navigationService.NavigateTo<TasksViewModel>());
        public ICommand SaveData => new Command(() => saveData());
        // а если не авторизован ? 
        private async void saveData()
        {
            if (TitleTask == "")
                TitleTask = Task.Name;
           var response = await _tasksService.PutTaskAddAsync(Task,TitleTask);
            if (response.IsSuccessStatusCode)
                _navigationService.NavigateTo<TasksViewModel>();
        }
        public ICommand DeleteTask => new Command(() => deleteTask());
        private async void deleteTask()
        {
            var response = await _tasksService.DeleteTaskAddAsync(TitleTask);
            if (response.IsSuccessStatusCode)
                _navigationService.NavigateTo<TasksViewModel>();
        }
    }

}
