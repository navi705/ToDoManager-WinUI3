using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using ToDoManager.Services.Navigation;
using ToDoManager.Services.Tasks;

namespace ToDoManager.ViewModels
{
    public class TasksViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITasksService _tasksService;
        public TasksViewModel(INavigationService navigationService, ITasksService tasksService)
        {
            _navigationService = navigationService;
            _tasksService = tasksService;
        }

        private List<ToDoTask> _toDoTasks;
        public List<ToDoTask> Tasks
        {
            get => _toDoTasks;
            set { _toDoTasks = value; OnPropertyChanged(nameof(Tasks)); }
        }
        private List<ToDoTask> _toDoTasksDaily;
        public List<ToDoTask> TasksDaily
        {
            get => _toDoTasksDaily;
            set { _toDoTasksDaily = value; OnPropertyChanged(nameof(TasksDaily)); }
        }
        private ObservableCollection<ToDoTask> _toDoTasksToday;
        public ObservableCollection<ToDoTask> TasksToday
        {
            get => _toDoTasksToday;
            set { _toDoTasksToday = value; OnPropertyChanged(nameof(TasksToday)); }
        }

        public ICommand command => new Command(() =>NavigateToEditOrAddTasksPage());

        public void NavigateToEditOrAddTasksPage()
        {
            ToDoTask newTask = new() {Name = "", Auto_fail = true, Date = DateTime.Now.Date.ToShortDateString(),Description="",
            Finish = false, Reapet="Simply",Time = DateTimeOffset.Now.TimeOfDay.ToString(@"hh\:mm")};
            _navigationService.NavigateTo<AddOrEditTasksViewModel>(newTask);
            
        }

        public async void Intilization()
        {
            HttpResponseMessage response = await _tasksService.GetTasksAsync();
            if (response.IsSuccessStatusCode)
            {
               var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
                var tasks = JsonSerializer.Deserialize<ToDoTasks>(response.Content.ReadAsStringAsync().Result.ToString(),options);
                Tasks= tasks.Tasks;
                TasksDaily = tasks.Tasks.Where(x=> x.Reapet == "Daily").ToList();
                TasksToday = new ObservableCollection<ToDoTask>( tasks.Tasks.Where(x => (x.Finish == false && x.Date == DateTimeOffset.Now.Date.ToShortDateString()) || (x.Finish == false) ).ToList());
                //GlobalVariables.ToDoTask = Tasks;

            }
            
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] = "";
                _navigationService.FrameBack();
                   

            }
            
        }   
        
        public void ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            _navigationService.NavigateTo<AddOrEditTasksViewModel>(args.InvokedItem);
        }

        public async void CompeteTask(ToDoTask task)
        {
           foreach(ToDoTask tasks in TasksToday)
           {
                if(task == tasks)
                {
                    tasks.Finish = true;
                    await _tasksService.PutTaskAddAsync(tasks, tasks.Name);
                    TasksToday.Remove(tasks);
                    break;
                }
           }
        }
    }
}