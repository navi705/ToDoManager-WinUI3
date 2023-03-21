using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using H.NotifyIcon;

namespace ToDoManager
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

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
            //RegisterBackgroundTask.RegisterBackgroundTasks(typeof(TaskCompletedSoon).FullName, typeof(TaskCompletedSoon).FullName, new TimeTrigger(15,false),null);
        }
        public static Window m_window;
        public static bool HandleClosedEvents { get; set; } = true;
    }
}
