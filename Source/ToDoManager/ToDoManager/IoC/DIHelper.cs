using ToDoManager.Services.Navigation;
using ToDoManager.ViewModels;
using TinyIoC;
using ToDoManager.Services.Authentication;
using ToDoManager.Services.Tasks;
using ToDoManager.Services.TimeManagerService;

namespace ToDoManager.IoC
{
    /// <summary>
    /// Dependency Injection Helper class 
    /// utilizing TinyIoC
    /// </summary>
    public class DIHelper
    {
        private static TinyIoCContainer _container;

        static DIHelper()
        {
            _container = new TinyIoCContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.Register<LoginViewModel>();
            _container.Register<RegisterViewModel>();
            _container.Register<ShellViewModel>();
            _container.Register<TasksViewModel>();
            _container.Register<AddOrEditTasksViewModel>();
            _container.Register<TimeMangerViewModel>();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IAuthenticationService,AuthenticationService>();
            _container.Register<ITasksService, TasksService>();
            _container.Register<ITimeManagerService, TimeManagerService>();
        }

        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }
    }
}
