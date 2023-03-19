using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        
        [HttpGet]
        [Authorize]
        [Route("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var tasks = await _mongoDbService.GetTasksAsync(email);
            return Ok(tasks);
        }

        [HttpPut]
        [Authorize]
        [Route("tasks")]
        public async Task<IActionResult> PutTask(string nametask,[FromBody] ToDoNote ask)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            await _mongoDbService.PutTaskAsync(nametask, ask,email);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("tasks")]
        public async Task<IActionResult> DeleteTask(string nameTask)
        {           
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            await _mongoDbService.DeleteTaskAsync(nameTask,email);
            return Ok();
        }
    }
}
