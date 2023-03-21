//using System;
//using System.IO;
//using System.Threading;
//using System.Runtime.InteropServices;

//namespace ToDoManager.Services.BackgroundTasks
//{
//    public class BackgroundTaskServer
//    {
//        BackgroundTaskServer()
//        {
//            comRegistrationToken = 0;
//            waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
//        }

//        ~BackgroundTaskServer()
//        {
//            Stop();
//        }

//        public void Start()
//        {
//            RegistrationServices registrationServices = new RegistrationServices();
//            comRegistrationToken = registrationServices.RegisterTypeForComClients(typeof(TaskCompletedSoon), RegistrationClassContext.LocalServer, RegistrationConnectionType.MultipleUse);

//            // Either have the background task signal this handle when it completes, or never signal this handle to keep this
//            // process as the COM server until the process is closed.
//            waitHandle.WaitOne();
//        }

//        public void Stop()
//        {
//            if (comRegistrationToken != 0)
//            {
//                RegistrationServices registrationServices = new RegistrationServices();
//                registrationServices.UnregisterTypeForComClients(registrationCookie);
//            }

//            waitHandle.Set();
//        }

//        private int comRegistrationToken;
//        private EventWaitHandle waitHandle;

//    }
//}
