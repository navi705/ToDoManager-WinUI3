using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using ToDoManager.IoC;
using ToDoManager.ViewModels;

namespace ToDoManager.Pages
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
        }

        public ShellViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = DIHelper.Resolve<ShellViewModel>();
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var menuItem = args.InvokedItemContainer.DataContext as MenuItem;
            if (menuItem.ViewModelType == null)
            {
                ViewModel.Logout();
            }
            else
            {
                ViewModel.SelectedPageChangedCommand.Execute(menuItem);
            }
        }
    }
}
