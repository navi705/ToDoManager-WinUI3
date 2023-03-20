using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ToDoManager.IoC;
using ToDoManager.Models;
using ToDoManager.ViewModels;

namespace ToDoManager.Pages
{
    public sealed partial class TasksPage : Page
    {
        public TasksPage()
        {
            this.InitializeComponent();
        }

        public TasksViewModel ViewModel { get; set; }

        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = DIHelper.Resolve<TasksViewModel>();
            ViewModel.Intilization();
        }

        private void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            var task = (ToDoTask)button.DataContext;
            ViewModel.CompeteTask(task);
        }
    }
}
