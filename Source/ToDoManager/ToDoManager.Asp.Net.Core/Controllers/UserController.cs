
using Microsoft.AspNetCore.Mvc;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Asp.Net.Core.Controllers
{
    //[Controller]
    //[Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public UserController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        //[HttpGet]
        protected async Task<List<User>> Get()
        {
            return await _mongoDbService.GetAsync();
        }

        
        protected async Task<List<User>> GetEmail(string Email)
        {
            return await _mongoDbService.GetAsyncEmail(Email);
        }

        //protected async Task AddToken()
        //{
            
        //}

       [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var user1 = await GetEmail(user.Email);
            if (user.Email != "" && user1.Count == 0)
            {
                await _mongoDbService.CreateAsync(user);
                //nameof(Get) раньше
                return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
            }
            else
            {
                return Conflict();
            }
            
        }
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(string id, [FromBody] string user)
        //{
        //    ;
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    await _mongoDbService.DeleteAsync(id);
        //    return NoContent();
        //}

    }
}
