using Microsoft.UI.Xaml;
using ToDoManager.Services.Navigation;
using ToDoManager.IoC;
using H.NotifyIcon.Core;

namespace ToDoManager
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {       
            this.InitializeComponent();
            
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);

            var navigationService = DIHelper.Resolve<INavigationService>();
            navigationService.InitializeFrame(MainFrame);           
        }
    }
}
