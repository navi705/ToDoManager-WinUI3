using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using H.NotifyIcon;
using H.NotifyIcon.Core;

namespace ToDoManager.Resources
{
    public sealed partial class TrayIconView : UserControl
    {
        public TrayIconView()
        {
            this.InitializeComponent();
        }

        public void ShowHideWindowCommand_ExecuteRequested(object? _, ExecuteRequestedEventArgs args)
        {
            var window = App.m_window;
            if (window == null)
            {
                return;
            }

            if (window.Visible)
            {
                window.Hide();
            }
            else
            {
                window.Show();
            }
        }

        public void ExitApplicationCommand_ExecuteRequested(object? _, ExecuteRequestedEventArgs args)
        {
            App.HandleClosedEvents = false;
            TrayIcon.Dispose();
            App.m_window?.Close();
        }
    }
}
