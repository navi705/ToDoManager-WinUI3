using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ToDoManager.Services.BackgroundTasks
{
    public static class RegisterBackgroundTask
    {
        public static BackgroundTaskRegistration RegisterBackgroundTasks( string taskEntryPoint,string name,IBackgroundTrigger trigger,IBackgroundCondition condition)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == name)
                {
                    //
                    // The task is already registered.
                    //

                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;

            // in-process background tasks don't set TaskEntryPoint
            if (taskEntryPoint != null && taskEntryPoint != String.Empty)
            {
                builder.TaskEntryPoint = taskEntryPoint;
            }
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }
    }
}
