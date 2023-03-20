using System.Net.Http;
using System.Threading.Tasks;
using ToDoManager.Models;

namespace ToDoManager.Services.TimeManagerService
{
    public interface ITimeManagerService
    {
        Task<HttpResponseMessage> GetTimeTableAsync();
        Task<HttpResponseMessage> PutTimeTableAsync(TimeNote time,string name);
        Task<HttpResponseMessage> DeleteTimeTableAsync(TimeNote time);
    }
}
