using System;
using System.Collections.Generic;
using ToDoManager.Models;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;


namespace ToDoManager.Services.BackgroundTasks
{
    public class FakeBackgroundTask
    {
        private readonly List<ToDoTask> _tasks;
        public FakeBackgroundTask(List<ToDoTask> tasks)
        {
            _tasks = tasks;
        }

        public void CheckTasksNotify(Object stateInfo)
        {
            if (_tasks == null || _tasks.Count == 0)
                return;
            foreach (ToDoTask task in _tasks)
            {
                if (task.Auto_fail == true)
                {
                    if (DateTimeOffset.Parse(task.Date) < DateTimeOffset.Now.Date)
                        continue;

                    if (DateTimeOffset.Parse(task.Date) == DateTimeOffset.Now.Date)
                    {
                        if (DateTimeOffset.Now.TimeOfDay - TimeSpan.Parse(task.Time) > TimeSpan.Parse("-00:30") && DateTimeOffset.Now.TimeOfDay - TimeSpan.Parse(task.Time) < TimeSpan.Parse("-00:20"))
                        {
                            AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;
                            //Not work bug
                            //AppNotificationManager.Default.Register();
                            var notification = new AppNotificationBuilder()
                                .AddText($"{task.Name} {task.Time}")
                                .BuildNotification();
                            AppNotificationManager.Default.Show(notification);
                            
                        }
                    }

                }
            }
        }
        private void OnNotificationInvoked(AppNotificationManager sender, AppNotificationActivatedEventArgs args)
        {
            // pass
        }
    }
}
