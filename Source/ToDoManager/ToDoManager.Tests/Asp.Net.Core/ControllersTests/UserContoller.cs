using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Tests.Asp.Net.Core.ControllersTests
{
    public class UserContoller
    {
       private static readonly Mock<MongoDbService> _mockDB = new(GlobalVariablesForTests.OptionMongoDbCon);
       private readonly ToDoManager.Asp.Net.Core.Controllers.UserController MockUserController = new(_mockDB.Object);

        [Fact]
        public async void Post_Successful()
        {
            var user = new ToDoManagerUser() { Email = "test@email.test",Password = "123456" };
            var user1 = new ToDoManagerUser() { Email = "test1@email1.test",Password = "123456" };

            CreatedAtActionResult CreatedAtResult = (CreatedAtActionResult) await MockUserController.Post(user);
            CreatedAtActionResult CreatedAtResult1 = (CreatedAtActionResult) await MockUserController.Post(user1);
           
            Assert.Equal(201, CreatedAtResult.StatusCode);
            Assert.Equal(201, CreatedAtResult1.StatusCode);
            var userForBd = await _mockDB.Object.GetUserForEmailAsync(user.Email);
            var userForBd1 = await _mockDB.Object.GetUserForEmailAsync(user1.Email);

            Assert.Equal(userForBd.Id, user.Id);
            Assert.Equal(userForBd1.Id, user1.Id);

            await _mockDB.Object.DeleteUserForEmailAsync(user.Email);
            await _mockDB.Object.DeleteUserForEmailAsync(user1.Email);
        }
        [Fact]
        public async void Post_Conflict()
        {
            var user = new ToDoManagerUser() { Email = "testconlict@email.test", Password = "123456" };
            var user1 = new ToDoManagerUser() { Email = "testconlict@email.test", Password = "123456" };

            CreatedAtActionResult CreatedAtResult = (CreatedAtActionResult)await MockUserController.Post(user);
            ConflictResult ConflictResult = (ConflictResult)await MockUserController.Post(user1);

            Assert.Equal(201, CreatedAtResult.StatusCode);
            Assert.Equal(409, ConflictResult.StatusCode);
            await _mockDB.Object.DeleteUserForEmailAsync(user.Email);
        }
    }
}
