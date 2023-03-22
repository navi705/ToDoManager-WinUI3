using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class TimeTablesController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly MongoDbService _mongoDbService;

        public TimeTablesController(MongoDbService mongoDbService, JwtService jwtService)
        {
            _jwtService = jwtService;
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        [Authorize]
        [Route("timetables")]
        public async Task<IActionResult> GetTimeTables()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var tasks = await _mongoDbService.GetTimeTablesAsync(email);
            return Ok(tasks);
        }

        [HttpPut]
        [Authorize]
        [Route("timetables")]
        public async Task<IActionResult> PutTimeTables(string name, [FromBody] TimeNote time)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            await  _mongoDbService.PutTimeTablesAsync(name,time,email);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("timetables")]
        public async Task<IActionResult> DelteTimeTables(string name)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            await _mongoDbService.DeleteTimeNoteAsync(name,email);
            return Ok();
        }
    }
}
