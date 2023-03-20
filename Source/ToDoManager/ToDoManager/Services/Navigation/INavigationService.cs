using ToDoManager.ViewModels;

namespace ToDoManager.Services.Navigation
{
    public interface INavigationService
    {
        void InitializeFrame(Microsoft.UI.Xaml.Controls.Frame rootFrame);

        void NavigateTo(System.Type page);

        void NavigateTo(System.Type page, object parameter);

        void FrameBack();

        void RemoveFromBackStack();
    }
}
