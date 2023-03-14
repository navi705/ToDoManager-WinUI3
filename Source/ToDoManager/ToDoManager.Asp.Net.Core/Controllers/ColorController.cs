using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class ColorController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly MongoDbService _mongoDbService;
        
        public ColorController(MongoDbService mongoDbService, JwtService jwtService)
        {
            _jwtService = jwtService;
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        [Authorize]
        [Route("color")]
        public async Task<IActionResult> GetColor()
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;

           return Ok(new { color = await _mongoDbService.GetColorAsync() });
        }

        [HttpPut]
        [Authorize]
        [Route("color")]
        public async Task<IActionResult> PutColor(string color)
        {
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await _mongoDbService.GetAsyncEmail(email);
            if (!user[0].Tokens.Contains(token))
            {
                return Unauthorized();
            }
            _mongoDbService.emailn = email;

            await _mongoDbService.UpdateColorAsync(color);

            return Ok();
        }
        
        }
}
