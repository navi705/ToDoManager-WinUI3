using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using Windows.ApplicationModel.Background;

namespace ToDoManager.Services.BackgroundTasks
{
    public class TaskCompletedSoon : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (GlobalVariables.ToDoTaskAddOrEdit == null)
                return;
            foreach(ToDoTask task in GlobalVariables.ToDoTaskAddOrEdit)
            {
                if (DateTimeOffset.Parse(task.Date) > DateTimeOffset.Now )
                {
                    task.Finish = true;
                }

                if (DateTimeOffset.Parse(task.Date) == DateTimeOffset.Now)
                {
                    if(TimeSpan.Parse(task.Time) > DateTimeOffset.Now.TimeOfDay)
                    {
                        task.Finish = true;
                    }

                    

                }

            }
        } 
    }
}
