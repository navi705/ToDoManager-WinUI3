// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using ToDoManager.IoC;
using ToDoManager.ViewModels;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ToDoManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
            Type pageType = typeof(SettingsPage);
            if (args.IsSettingsInvoked)
            {
                MainContentFrame.NavigateToType(pageType, null,null);
            }
            else
            {
                var menuItem = args.InvokedItemContainer.DataContext as MenuItem;
                ViewModel.SelectedPageChangedCommand.Execute(menuItem);
            }
        }
    }
}
