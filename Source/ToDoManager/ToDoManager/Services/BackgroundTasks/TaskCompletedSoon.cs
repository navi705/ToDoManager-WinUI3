using System;
using System.Linq;
using System.Runtime.InteropServices;
using ToDoManager.HelpClasses;
using ToDoManager.Models;
using Windows.ApplicationModel.Background;

namespace ToDoManager.Services.BackgroundTasks
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("56e48bc6-a15b-4e63-9495-a6cf6f788bce")]
    [ComSourceInterfaces(typeof(IBackgroundTask))]
    public class TaskCompletedSoon : IBackgroundTask
    {
        [MTAThread]
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
