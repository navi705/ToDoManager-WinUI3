using System;
using System.Linq;
using System.Windows.Input;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using ToDoManager.Services.Navigation;
using ToDoManager.Services.Tasks;

namespace ToDoManager.ViewModels
{
    public class AddOrEditTasksViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITasksService _tasksService;
        public AddOrEditTasksViewModel(INavigationService navigationService, ITasksService tasksService)
        {
            _navigationService = navigationService;
            _tasksService = tasksService;
        }

        public ToDoTask Task { get; set; }
        public string TitleTask { get; set;}
        public DateTimeOffset Date
        {
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

        public ICommand ToTaskPage => new Command(() => CancelButton());

        private void CancelButton()
        {
            if (GlobalVariables.ToDoTaskAddOrEdit.Count == 0)
            {
                GlobalVariables.ToDoTaskAddOrEdit.Clear();
                _navigationService.NavigateTo(typeof(TasksViewModel));
            }
            else
            {
                var task = GlobalVariables.ToDoTaskAddOrEdit.Last();
                GlobalVariables.ToDoTaskAddOrEdit.Remove(task);
                _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel),task);
            }
        }

        public ICommand SaveData => new Command(() => saveData());
        private async void saveData()
        {
            if (Task.Name == "")
                return;
            if (TitleTask == "")
                TitleTask = Task.Name;
            if (GlobalVariables.ToDoTaskAddOrEdit.Count == 0)
            {
                var response = await _tasksService.PutTaskAddAsync(Task, TitleTask);
                if (response.IsSuccessStatusCode)
                    GlobalVariables.ToDoTaskAddOrEdit.Clear();
                    _navigationService.NavigateTo(typeof(TasksViewModel));              
            }
            else
            {
                if(GlobalVariables.ToDoTaskAddOrEdit.Last().Subtasks == null)
                    GlobalVariables.ToDoTaskAddOrEdit.Last().Subtasks.Add(Task);

                if (!GlobalVariables.ToDoTaskAddOrEdit.Last().Subtasks.Contains(Task) )
                    GlobalVariables.ToDoTaskAddOrEdit.Last().Subtasks.Add(Task); 
                var task = GlobalVariables.ToDoTaskAddOrEdit.Last();
                GlobalVariables.ToDoTaskAddOrEdit.Remove(task);
                _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel), task);
            }
        }
        public ICommand DeleteTask => new Command(() => deleteTask());
        private async void deleteTask()
        {
            if (GlobalVariables.ToDoTaskAddOrEdit.Count == 0)
            {
                var response = await _tasksService.DeleteTaskAddAsync(TitleTask);
                if (response.IsSuccessStatusCode)
                    GlobalVariables.ToDoTaskAddOrEdit.Clear();
                    _navigationService.NavigateTo(typeof(TasksViewModel));        
            }
            else
            {
                GlobalVariables.ToDoTaskAddOrEdit.Last().Subtasks.Remove(Task);
                var task = GlobalVariables.ToDoTaskAddOrEdit.Last();
                GlobalVariables.ToDoTaskAddOrEdit.Remove(task);
                _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel),task);
            }
               
        }
        public ICommand AddSubTask => new Command(() => addSubTask());
        private void addSubTask()
        {
            GlobalVariables.ToDoTaskAddOrEdit.Add(Task);
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
            _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel),newTask);
        }
        public void SelectedSubTask(ToDoTask toDoTask)
        {
            GlobalVariables.ToDoTaskAddOrEdit.Add(Task);
            _navigationService.NavigateTo(typeof(AddOrEditTasksViewModel),toDoTask);
        }
    }
}
