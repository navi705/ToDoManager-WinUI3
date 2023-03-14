using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System.Net.Http;
using ToDoManager.ViewModels;


namespace ToDoManager.Services.Navigation
{
    public interface INavigationService
    {

        void InitializeFrame(Microsoft.UI.Xaml.Controls.Frame rootFrame);

        void NavigateTo<T>() where T : ViewModelBase;
        //why type trought <> but not Type ?
        void NavigateTo<T>(object parameter) where T : ViewModelBase;

        void NavigateTo(System.Type page);

        void NavigateTo(System.Type page, object parameter);

        void FrameBack();

        void RemoveFromBackStack();
    }
}
