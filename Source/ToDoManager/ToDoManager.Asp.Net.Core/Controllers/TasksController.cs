
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class TasksController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly MongoDbService _mongoDbService;
        
        public TasksController(MongoDbService mongoDbService, JwtService jwtService) 
        {
            _jwtService = jwtService;
            _mongoDbService = mongoDbService;
        }

        [HttpPost]
        [Authorize]
        [Route("task/add")]
        public async Task<IActionResult> AddTask(NotMultiTask task)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
            await _mongoDbService.AddTaskAsync(task);
            return Ok();

        }
        
        [HttpGet]
        [Authorize]
        [Route("task")]
        public async Task<IActionResult> GetTasks()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
            var a = await _mongoDbService.GetTasksAsync();
            //return Ok(new { tasks = await _mongoDbService.GetTasksAsync() }) ;
            return Ok(a);
        }

        [HttpPut]
        [Authorize]
        [Route("task")]
        public async Task<IActionResult> PutTask(string nametask,[FromBody] NotMultiTask ask)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
            await _mongoDbService.PutTaskAsync(nametask, ask);
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        [Route("task")]
        public async Task<IActionResult> DeleteTask(string nameTask)
        {           
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;

            await _mongoDbService.DeleteTaskAsync(nameTask);

            return Ok();
            
            
        }

    }
}
