using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ToDoManager.Services.Navigation;
using ToDoManager.IoC;

namespace ToDoManager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
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
