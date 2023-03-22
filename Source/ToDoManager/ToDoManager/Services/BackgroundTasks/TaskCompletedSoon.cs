using System;
using System.Linq;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using Windows.ApplicationModel.Background;

namespace ToDoManager.Services.BackgroundTasks
{
    public  class TaskCompletedSoon : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (GlobalVariables.ToDoTasks == null)
                return;
            var tasks = GlobalVariables.ToDoTaskAddOrEdit.Where(x => x.Date == DateTimeOffset.Now.Date.ToShortDateString()).ToList();
            foreach (ToDoTask task in tasks)
            {
                if (DateTimeOffset.Parse(task.Date) > DateTimeOffset.Now )
                {
                    task.Finish = true;
                }

                if (DateTimeOffset.Parse(task.Date) == DateTimeOffset.Now)
                {
                    if (TimeSpan.Parse(task.Time) > DateTimeOffset.Now.TimeOfDay)
                    {
                        task.Finish = true;
                    }
                }
            }
        } 
    }
}
