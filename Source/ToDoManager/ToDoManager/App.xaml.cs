using Microsoft.UI.Xaml;
using H.NotifyIcon;
using ToDoManager.Services.BackgroundTasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Activation;

namespace ToDoManager
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
           
        }

        public static Window m_window;
        public static bool HandleClosedEvents { get; set; } = true;

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.TryAcrylicBackdrops();

            m_window.Closed += (sender, args) =>
            {
                if (HandleClosedEvents)
                {
                    args.Handled = true;
                    m_window.Hide();
                }
            };
            m_window.Activate();
        }


    }
}
