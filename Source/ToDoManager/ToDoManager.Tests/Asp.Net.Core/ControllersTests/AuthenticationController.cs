using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Models.ModelForController;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Tests.Asp.Net.Core.ControllersTests
{
    public class AuthenticationController
    {
        private static readonly Mock<MongoDbService> _mockDB = new(GlobalVariablesForTests.OptionMongoDbCon);
        private static readonly Mock<JwtService> _mockJwt = new(GlobalVariablesForTests.OptionJwt);
        private readonly ToDoManager.Asp.Net.Core.Controllers.AuthenticationController _autController = new(_mockDB.Object,_mockJwt.Object);

        [Fact]
        public async void Login_UnauthorizedUserNotFound()
        {
            var user = new AuthenticationModel() { Email = "testlogin@email.test", Password = "123456" };

            UnauthorizedResult result = (UnauthorizedResult) await _autController.Login(user);

            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async void Login_UnauthorizedPasswordNotCorrect()
        {
            var user = new ToDoManagerUser() { Email = "testlogin@email.test", Password = "123456" };
            await _mockDB.Object.CreateUserAsync(user);
            var user1 = new AuthenticationModel() { Email = "testlogin@email.test", Password = "12345" };

            UnauthorizedResult result = (UnauthorizedResult)await _autController.Login(user1);

            Assert.Equal(401, result.StatusCode);
            await _mockDB.Object.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void Login_Ok()
        {
            var user = new ToDoManagerUser() { Email = "testloginok@email.test", Password = "123456" };
            await _mockDB.Object.CreateUserAsync(user);
            var user1 = new AuthenticationModel() { Email = "testloginok@email.test", Password = "123456" };

            OkObjectResult result = (OkObjectResult)await _autController.Login(user1);
            Assert.Equal(200, result.StatusCode);

            await _mockDB.Object.DeleteUserForEmailAsync(user.Email);
        }
    }
}
