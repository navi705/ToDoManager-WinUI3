using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Tests.Asp.Net.Core.ControllersTests
{
    // bad practice ?
    public class TasksController
    {
        private static readonly MongoDbService _mockDB = new(GlobalVariablesForTests.OptionMongoDbCon);

        [Fact]
        public async void PutTaksks_Ok()
        {
            ToDoNote toDoNote = new() { Name = "Drink beer", Date = "2023-03-19", Description = "Beer is cool", Finish = false, Time = "14:14", Reapet = "Daily"};
            ToDoNote toDoNote1 = new() { Name = "Drink vodka", Date = "2023-03-20", Description = "Vodka is cool", Finish = false, Time = "14:15", Reapet = "Daily"};
            var user = new ToDoManagerUser() { Email = "testPut@email.test", Password = "123456" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTaskAsync("", toDoNote,user.Email);
            await _mockDB.PutTaskAsync("", toDoNote1,user.Email);

            var getUser = await _mockDB.GetUserForEmailAsync(user.Email);
            Assert.Equal(toDoNote.Name, getUser.ToDoTasks[0].Name);
            Assert.Equal(toDoNote1.Name, getUser.ToDoTasks[1].Name);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void PutTaksks_OkChangeName()
        {
            var user = new ToDoManagerUser() { Email = "testPutTasks@changeName.test", Password = "123456" };
            ToDoNote toDoNote = new() { Name = "Drink beer", Date = "2023-03-19", Description = "Beer is cool", Finish = false, Time = "14:14", Reapet = "Daily" };
            ToDoNote toDoNote1 = new() { Name = "Drink vodka", Date = "2023-03-20", Description = "Vodka is cool", Finish = false, Time = "14:15", Reapet = "Daily" };
            ToDoNote toDoNote2 = new() { Name = "Drink water", Date = "2023-03-21", Description = "Water is not cool", Finish = false, Time = "14:15", Reapet = "Daily" };
            ToDoNote toDoNote3 = new() { Name = "Drink beers", Date = "2023-03-22", Description = "Beer is not cool", Finish = false, Time = "16:14", Reapet = "Daily" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTaskAsync("", toDoNote, user.Email);
            await _mockDB.PutTaskAsync("", toDoNote1, user.Email);
            await _mockDB.PutTaskAsync("", toDoNote2, user.Email);
            await _mockDB.PutTaskAsync("Drink beer", toDoNote3, user.Email);
            var getUser = await _mockDB.GetUserForEmailAsync(user.Email);

            Assert.Equal(toDoNote3.Name, getUser.ToDoTasks[0].Name);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void GetTaksks_Ok()
        {
            var user = new ToDoManagerUser() { Email = "testGetTasks@changeName.test", Password = "123456" };
            ToDoNote toDoNote = new() { Name = "Drink beer", Date = "2023-03-19", Description = "Beer is cool", Finish = false, Time = "14:14", Reapet = "Daily" };
            ToDoNote toDoNote1 = new() { Name = "Drink vodka", Date = "2023-03-20", Description = "Vodka is cool", Finish = false, Time = "14:15", Reapet = "Daily" };
            ToDoNote toDoNote2 = new() { Name = "Drink water", Date = "2023-03-21", Description = "Water is not cool", Finish = false, Time = "14:15", Reapet = "Daily" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTaskAsync("", toDoNote, user.Email);
            await _mockDB.PutTaskAsync("", toDoNote1, user.Email);
            await _mockDB.PutTaskAsync("", toDoNote2, user.Email);
            var getUser = await _mockDB.GetTasksAsync(user.Email);
            Assert.Equal(toDoNote.Name, getUser[0].Name);
            Assert.Equal(toDoNote1.Name, getUser[1].Name);
            Assert.Equal(toDoNote2.Name, getUser[2].Name);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void DeleteTaksks_Ok()
        {
            var user = new ToDoManagerUser() { Email = "testGetTasks@changeName.test", Password = "123456" };
            ToDoNote toDoNote = new() { Name = "Drink beer", Date = "2023-03-19", Description = "Beer is cool", Finish = false, Time = "14:14", Reapet = "Daily" };
            ToDoNote toDoNote1 = new() { Name = "Drink vodka", Date = "2023-03-20", Description = "Vodka is cool", Finish = false, Time = "14:15", Reapet = "Daily" };
            await _mockDB.CreateUserAsync(user);
            await _mockDB.PutTaskAsync("", toDoNote, user.Email);
            await _mockDB.PutTaskAsync("", toDoNote1, user.Email);

            await _mockDB.DeleteTaskAsync(toDoNote.Name, user.Email);
            await _mockDB.DeleteTaskAsync(toDoNote1.Name, user.Email);
            var getUser = await _mockDB.GetUserForEmailAsync(user.Email);

            Assert.Empty(getUser.ToDoTasks);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }
    }
}
