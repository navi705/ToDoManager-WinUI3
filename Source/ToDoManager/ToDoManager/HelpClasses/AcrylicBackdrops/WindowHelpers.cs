using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Windows.UI.ViewManagement;
using WinRT;
using System;
using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace ToDoManager
{
    public static class WindowHelpers
    {
        public static void TryAcrylicBackdrops(this Microsoft.UI.Xaml.Window window)
        {
            var dispatcherQueueHelper = new WindowsSystemDispatcherQueueHelper(); 
            dispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            var configurationSource = new SystemBackdropConfiguration();
            configurationSource.IsInputActive = true;

            switch (((FrameworkElement)window.Content).ActualTheme)
            {
                case ElementTheme.Dark:
                    configurationSource.Theme = SystemBackdropTheme.Dark;
                    break;
                case ElementTheme.Light:
                    configurationSource.Theme = SystemBackdropTheme.Light;
                    break;
                case ElementTheme.Default:
                    configurationSource.Theme = SystemBackdropTheme.Default;
                    break;
            }



            if (DesktopAcrylicController.IsSupported())
            {
                //window.ExtendsContentIntoTitleBar = true;               
                //settings backdrops for the window
                var acrylicController = new DesktopAcrylicController();
                //acrylicController.LuminosityOpacity = 0.5f;
                //acrylicController.TintOpacity = 0.7f;
                acrylicController.LuminosityOpacity = 0.3f;
                acrylicController.TintOpacity = 0.6f;
                var uiSettings = new UISettings();
                var color = uiSettings.GetColorValue(UIColorType.AccentDark2);
                acrylicController.TintColor = color;
                acrylicController.FallbackColor = color;
                
                acrylicController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>());
                acrylicController.SetSystemBackdropConfiguration(configurationSource);

                //settings title bar

                //window.Title = "";
                //IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

                //WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                //Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                //Microsoft.UI.Windowing.AppWindowTitleBar titlebar = appWindow.TitleBar;

                //titlebar.BackgroundColor = color;
                //titlebar.ButtonBackgroundColor = Colors.Transparent;
                //titlebar.ButtonForegroundColor = Colors.Transparent;

                //titlebar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;


                window.Activated += (object sender, WindowActivatedEventArgs args) =>
                {
                    if (args.WindowActivationState is WindowActivationState.CodeActivated or WindowActivationState.PointerActivated)
                    {
                        // Handle situation where a window is activated and placed on top of other active windows.
                        if (acrylicController == null)
                        {
                            acrylicController = new DesktopAcrylicController();
                            acrylicController.AddSystemBackdropTarget(window.As<ICompositionSupportsSystemBackdrop>()); // Requires 'using WinRT;'
                            acrylicController.SetSystemBackdropConfiguration(configurationSource);
                        }
                    }

                    if (configurationSource != null)
                        configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;                       
                };

                window.Closed += (object sender, WindowEventArgs args) =>
                {
                    if (acrylicController != null)
                    {
                        acrylicController.Dispose();
                        acrylicController = null;
                    }

                    configurationSource = null;
                };
            }

        }

    }
}
