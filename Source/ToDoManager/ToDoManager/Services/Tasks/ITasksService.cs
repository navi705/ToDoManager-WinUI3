using System.Net.Http;
using System.Threading.Tasks;
using ToDoManager.Models;

namespace ToDoManager.Services.Tasks
{
    public interface ITasksService
    {
        Task<HttpResponseMessage> GetTasksAsync();
        Task<HttpResponseMessage> PutTaskAddAsync(ToDoTask toDoTask,string nameTask);
        Task<HttpResponseMessage> DeleteTaskAddAsync(string nameTask);
    }
}
