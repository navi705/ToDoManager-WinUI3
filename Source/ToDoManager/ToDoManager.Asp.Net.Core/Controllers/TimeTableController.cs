using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class TimeTableController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly MongoDbService _mongoDbService;

        public TimeTableController(MongoDbService mongoDbService, JwtService jwtService)
        {
            _jwtService = jwtService;
            _mongoDbService = mongoDbService;
        }

        [HttpPost]
        [Authorize]
        [Route("timetable/add")]
        public async Task<IActionResult> AddTimeTable(Time time)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;

            await _mongoDbService.AddTimeTableAsync(time);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("timetables")]
        public async Task<IActionResult> GetTimeTables()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
            // return Ok(new { tasks = await _mongoDbService.GetTimeTablesAsync() });
            var tasks = await _mongoDbService.GetTimeTablesAsync();
            return Ok(tasks);
        }

        [HttpPut]
        [Authorize]
        [Route("timetable")]
        public async Task<IActionResult> PutTimeTable(string name, [FromBody] Time time)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
           await  _mongoDbService.PutTimeTableAsync(name,time);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("timetable")]
        public async Task<IActionResult> DelteTimeTable(string name)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;
            await _mongoDbService.DeleteTimeTableAsync(name);
            return Ok();
        }
    }
}
