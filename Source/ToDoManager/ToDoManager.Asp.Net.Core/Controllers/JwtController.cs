using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    [Controller]
    [Route("jwt/[controller]")]
    public class JwtController: UserController
    {
        private readonly JwtService _jwtService;
        private readonly MongoDbService _mongoDbService;
        public JwtController(MongoDbService mongoDbService, JwtService jwtService) : base(mongoDbService)
        {
            _jwtService = jwtService;
            _mongoDbService = mongoDbService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password,string device)
        {
            var user = await GetEmail(email);
            if (user.Count == 0)
            {
                return NotFound();
            }
            if (user[0].Password != password)
            {
                return Unauthorized();
            }
            string Token = _jwtService.GenerateToken(email);
            await _mongoDbService.AddTokenAsync(email, Token, device);
            return Ok(new { token = Token });
        }
        
        [HttpPost]
        [Authorize]
        [Route("check")]
        public async Task<IActionResult> CheckToken()
        {
           string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            string email = _jwtService.DecodeToken(token);
            var user = await  GetEmail(email);
            
            
            if (user[0].Tokens.Contains(token))
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }       
        }

    }
}
    

