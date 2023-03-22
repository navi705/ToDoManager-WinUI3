using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoManager.HelpClasses;
using ToDoManager.Models;

namespace ToDoManager.Services.Tasks
{
    public class TasksService : ITasksService
    {
        private HttpClient _httpClient = new();

        public async Task<HttpResponseMessage> GetTasksAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync($"{GlobalVariables._baseAddres}" + "tasks");
            }
            catch
            {
                return null;
            }
            return response;
        }

        public async Task<HttpResponseMessage> PutTaskAddAsync(ToDoTask toDoTask,string nameTask)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response ;
            try
            {
                response = await _httpClient.PutAsync($"{GlobalVariables._baseAddres}" + $"tasks?nameTask={nameTask}", JsonContent.Create(toDoTask));
            }
            catch
            {
                return null;
            }
            return response;
        }

        public async Task<HttpResponseMessage> DeleteTaskAddAsync(string nameTask)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.DeleteAsync($"{GlobalVariables._baseAddres}" + $"tasks?nameTask={nameTask}");
            }
            catch
            {
                return null;
            }
            return response;
        }
    }
}
