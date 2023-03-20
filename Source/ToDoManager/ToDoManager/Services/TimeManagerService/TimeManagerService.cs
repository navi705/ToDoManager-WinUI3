using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDoManager.HelpClasses;
using ToDoManager.Models;

namespace ToDoManager.Services.TimeManagerService
{
    public class TimeManagerService : ITimeManagerService
    {
        private HttpClient _httpClient = new();

        public async Task<HttpResponseMessage> GetTimeTableAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync($"{GlobalVariables._baseAddres}" + "timetables");
            }
            catch
            {
                return null;
            }
            return response;
        }

        public async Task<HttpResponseMessage> PutTimeTableAsync(TimeNote time,string name)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PutAsync($"{GlobalVariables._baseAddres}" + $"timetables?name={name}", JsonContent.Create(time));
            }
            catch
            {
                return null;
            }
            return response;
        }

        public async Task<HttpResponseMessage> DeleteTimeTableAsync(TimeNote time)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Windows.Storage.ApplicationData.Current.LocalSettings.Values["token"].ToString());
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.DeleteAsync($"{GlobalVariables._baseAddres}" + $"timetables?name={time.NameTask}");
            }
            catch
            {
                return null;
            }
            return response;
        }
    }
}
