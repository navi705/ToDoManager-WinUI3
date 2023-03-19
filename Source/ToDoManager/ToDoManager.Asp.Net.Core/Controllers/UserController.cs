using Microsoft.AspNetCore.Mvc;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    public class UserController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public UserController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
  
        [HttpPost("users")]
        public async Task<IActionResult> Post([FromBody] ToDoManagerUser user)
        {
            if(await _mongoDbService.CreateUserAsync(user))
                return CreatedAtAction(nameof(Post), user.Email);
            else
                return Conflict();           
        }
    }
}
