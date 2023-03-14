using System.Net.Http;
using System.Threading.Tasks;
using ToDoManager.Models;

namespace ToDoManager.Services.TimeManagerService
{
    public interface ITimeManagerService
    {
        Task<HttpResponseMessage> GetTimeTableAsync();
        Task<HttpResponseMessage> PutTimeTableAsync(Time time,string name);
        Task<HttpResponseMessage> DeleteTimeTableAsync(Time time);
    }
}
