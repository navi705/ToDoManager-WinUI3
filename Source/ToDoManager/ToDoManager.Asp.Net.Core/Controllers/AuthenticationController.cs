using Microsoft.AspNetCore.Mvc;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Models.ModelForController;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MongoDbService _mongoDbService;
        private readonly JwtService _jwtService;

        public AuthenticationController(MongoDbService mongoDbService, JwtService jwtService)
        {
            _mongoDbService = mongoDbService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationModel userFromRequired )
        {
            ToDoManagerUser user = await _mongoDbService.GetUserForEmailAsync(userFromRequired.Email);
            if (user.Email == null )
                return Unauthorized();
            if (user.Password != userFromRequired.Password)
                return Unauthorized();
            string token = _jwtService.GenerateToken(userFromRequired.Email);
            return Ok(token);
        }
    }
}
