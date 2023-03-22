using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoManager.Models;
using ToDoManager.Services.BackgroundTasks;
using ToDoManager.Services.Navigation;
using ToDoManager.Services.Tasks;
using ToDoManager.Services.TimeManagerService;

namespace ToDoManager.ViewModels
{
    public class TasksViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITasksService _tasksService;
        private readonly ITimeManagerService _timeService;
        public TasksViewModel(INavigationService navigationService, ITasksService tasksService, ITimeManagerService timeService)
        {
            _navigationService = navigationService;
            _tasksService = tasksService;
            _timeService = timeService;
        }

        private List<ToDoTask> _toDoTasks;
        public List<ToDoTask> Tasks
        {
            get => _toDoTasks;
            set { _toDoTasks = value; OnPropertyChanged(nameof(Tasks)); }
        }
        private ObservableCollection<ToDoTask> _toDoTasksDaily;
        public ObservableCollection<ToDoTask> TasksDaily
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

        private ObservableCollection<ToDoTask> _toDoTasksTomorrow;
        public ObservableCollection<ToDoTask> TasksTomorrow
        {
            get => _toDoTasksTomorrow;
            set { _toDoTasksTomorrow = value; OnPropertyChanged(nameof(TasksTomorrow)); }
        }

        private ObservableCollection<ToDoTask> _toDoTasksMouth;
        public ObservableCollection<ToDoTask> TasksMouth
        {
            get => _toDoTasksMouth;
            set { _toDoTasksMouth = value; OnPropertyChanged(nameof(TasksMouth)); }
        }

        private ObservableCollection<ToDoTask> _toDoTasksSimply;
        public ObservableCollection<ToDoTask> TasksSimply
        {
            get => _toDoTasksSimply;
            set { _toDoTasksSimply = value; OnPropertyChanged(nameof(TasksSimply)); }
        }

        public ICommand command => new Command(() => NavigateToEditOrAddTasksPage());

        public void NavigateToEditOrAddTasksPage()
        {
            ToDoTask newTask = new()
            {
                Name = "",
                Auto_fail = true,
                Date = DateTime.Now.Date.ToShortDateString(),
                Description = "",
                Finish = false,
                Reapet = "Simple Repeat",
                Time = DateTimeOffset.Now.TimeOfDay.ToString(@"hh\:mm")
            };
            _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel), newTask);

        }

        public async void Intilization()
        {
            HttpResponseMessage response = await _tasksService.GetTasksAsync();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
                var tasksFromRequest = JsonSerializer.Deserialize<List<ToDoTask>>(response.Content.ReadAsStringAsync().Result.ToString(), options);

                var tasks = checkTasks(tasksFromRequest);
                var fakeBackgroundTask = new FakeBackgroundTask(tasks);
                var stateTimer = new Timer(fakeBackgroundTask.CheckTasksNotify, new AutoResetEvent(false), 0, 600000);

                Tasks = tasks;
                TasksDaily = new ObservableCollection<ToDoTask>(tasks.Where(x => x.Reapet == "Daily").ToList());
                TasksSimply = new ObservableCollection<ToDoTask>(tasks.Where(x => x.Finish == false && x.Reapet == "Simple Repeat").ToList());
                TasksMouth = new ObservableCollection<ToDoTask>(tasks.Where(x => x.Reapet == "Mouthly").ToList());
                TasksToday = new ObservableCollection<ToDoTask>(tasks.Where(x => (x.Finish == false && x.Date == DateTimeOffset.Now.Date.ToShortDateString())).ToList());
                TasksTomorrow = new ObservableCollection<ToDoTask>(tasks.Where(x => (x.Finish == false &&
                x.Date == DateTimeOffset.Now.AddDays(1).Date.ToShortDateString())).ToList());
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] = "";
                _navigationService.FrameBack();
            }
        }

        public void ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var task = (ToDoTask)args.InvokedItem;
            if (IsNotChildTask(Tasks, task.Name))
                _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel), task);
        }

        public async void CompeteTask(ToDoTask task)
        {
            var collection = getCollection(task);
            foreach (ToDoTask tasks in collection)
            {
                if (task == tasks)
                {
                    if (task.Reapet == "Simple Repeat")
                    {
                        await _tasksService.DeleteTaskAddAsync(task.Name);
                        collection.Remove(tasks);
                        addTimeNote(tasks);
                        return;
                    }
                    tasks.Finish = true;
                    await _tasksService.PutTaskAddAsync(tasks, tasks.Name);
                    collection.Remove(tasks);
                    addTimeNote(tasks);
                    break;
                }
            }
        }

        private ObservableCollection<ToDoTask> getCollection(ToDoTask task)
        {
            if (task.Reapet == "Simple Repeat")
                return TasksSimply;
            if (task.Date == DateTimeOffset.Now.Date.ToShortDateString())
                return TasksToday;
            return null;
        }

        private async void addTimeNote(ToDoTask task)
        {
            TimeNote timeNote = new() { NameTask = task.Name, Date = task.Date, Of = task.Time, To = DateTimeOffset.Now.TimeOfDay.ToString(@"hh\:mm") };
            await _timeService.PutTimeTableAsync(timeNote, "");
        }

        private bool IsNotChildTask(List<ToDoTask> tasks, string nameTask)
        {
            foreach (var task in tasks)
            {
                if (task.Name == nameTask)
                {
                    return true;
                }
                if (task.Subtasks == null)
                {
                    continue;
                }
            }
            return false;
        }

        private List<ToDoTask> checkTasks(List<ToDoTask> tasks)
        {
            if (tasks == null || tasks.Count == 0)
                return new List<ToDoTask>();
            foreach (ToDoTask task in tasks)
            {
                if (task.Auto_fail == true)
                {
                    if (DateTimeOffset.Parse(task.Date) < DateTimeOffset.Now.Date)
                    {
                        if (task.Reapet != "Simple Repeat")
                            task.Date = DateTime.Now.Date.ToShortDateString();
                        task.Finish = true;
                    }

                    if (DateTimeOffset.Parse(task.Date) == DateTimeOffset.Now.Date)
                    {
                        if (TimeSpan.Parse(task.Time) < DateTimeOffset.Now.TimeOfDay)
                        {
                            task.Finish = true;
                        }
                    }
                }
                // reset tasks
                if (task.Reapet != "Simple Repeat")
                {
                    if (task.Reapet == "Daily")
                    {
                        if (DateTimeOffset.Parse(task.Date) < DateTimeOffset.Now.Date)
                            task.Finish = false;
                    }

                    if (task.Reapet == "Weekly")
                    {
                        if (DateTimeOffset.Parse(task.Date) < DateTimeOffset.Now.Date.AddDays(7))
                            task.Finish = false;
                    }

                    if (task.Reapet == "Mouthly")
                    {
                        if (DateTimeOffset.Parse(task.Date) < DateTimeOffset.Now.Date.AddDays(30))
                            task.Finish = false;
                    }
                }
            }
            return tasks;
        }

    }
}