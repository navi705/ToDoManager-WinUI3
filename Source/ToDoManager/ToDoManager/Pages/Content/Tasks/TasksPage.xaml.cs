// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ToDoManager.IoC;
using ToDoManager.Models;
using ToDoManager.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
