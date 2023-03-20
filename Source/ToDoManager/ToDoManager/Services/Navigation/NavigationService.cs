using ToDoManager.Pages;
using ToDoManager.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ToDoManager.Services.Navigation;
using Microsoft.UI.Xaml.Media.Animation;

namespace ToDoManager.Services.Navigation
{
    class NavigationService : INavigationService
    {
        private Frame _shellFrame;
        private Frame _mainFrame;
        
        public void InitializeFrame(Frame rootFrame)
        {
            _shellFrame = rootFrame;
            _mainFrame = rootFrame;
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] == "" || Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"] == null)
            {
                NavigateTo(typeof(LoginViewModel));
            }
            else
            {
                NavigateTo(typeof(ShellViewModel));
            }
            //NavigateTo<LoginViewModel>();
        }

        public void NavigateTo(Type page) 
        {
            InternalNavigateTo(page, null);
        }

        public void NavigateTo(Type page,object parameter)
        {
            InternalNavigateTo(page,parameter);
        }

        public void RemoveFromBackStack()
        {
            _shellFrame?.BackStack.Remove(_shellFrame.BackStack.Last());
        }


        private void InternalNavigateTo(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);
            _shellFrame?.Navigate(pageType, parameter);

            var content = _shellFrame.Content;
            if (content is ShellPage shellPage)
            {
                var navigationView = (shellPage.Content as Panel).Children.OfType<NavigationView>().First();
                var navFrame = (navigationView.Content as Panel).Children.OfType<Frame>().First();
                _shellFrame = navFrame;

                // navigate to book flight viewmodel //default
                NavigateTo(typeof(TasksViewModel));
            }
        }

        public void FrameBack() {
            _shellFrame = _mainFrame ;
            NavigateTo(typeof(LoginViewModel));
        }
        
        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }
    }
}
