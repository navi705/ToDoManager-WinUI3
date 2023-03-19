using Moq;
using ToDoManager.Asp.Net.Core.Models;
using ToDoManager.Asp.Net.Core.Services;

namespace ToDoManager.Tests.Asp.Net.Core.ControllersTests
{
    public class TimeTableCotroller
    {
        private static readonly MongoDbService _mockDB = new(GlobalVariablesForTests.OptionMongoDbCon);

        [Fact]
        public async void PutTimeTable_OK()
        {
            TimeNote timeNote = new() { NameTask="Drink beer", Date="2023-03-20", Of ="10:55", To="12:55" };
            TimeNote timeNote1 = new() { NameTask="Drink vodka", Date="2023-03-20", Of ="13:55", To="14:55" };
            var user = new ToDoManagerUser() { Email = "testPut@email.test", Password = "123456" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTimeTablesAsync("", timeNote, user.Email);
            await _mockDB.PutTimeTablesAsync("", timeNote1, user.Email);

            var getUser = await _mockDB.GetUserForEmailAsync(user.Email);
            Assert.Equal(timeNote.NameTask, getUser.TimeTable[0].NameTask);
            Assert.Equal(timeNote1.NameTask, getUser.TimeTable[1].NameTask);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void PutTimeTable_OKChangeName()
        {
            TimeNote timeNote = new() { NameTask = "Drink beer", Date = "2023-03-20", Of = "10:55", To = "12:55" };
            TimeNote timeNote1 = new() { NameTask = "Drink vodka", Date = "2023-03-20", Of = "13:55", To = "14:55" };
            var user = new ToDoManagerUser() { Email = "testPut@email.test", Password = "123456" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTimeTablesAsync("", timeNote, user.Email);
            await _mockDB.PutTimeTablesAsync("Drink beer", timeNote1, user.Email);

            var getUser = await _mockDB.GetUserForEmailAsync(user.Email);
            Assert.Equal(timeNote1.NameTask, getUser.TimeTable[0].NameTask);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void GetTimeTable_OK()
        {
            TimeNote timeNote = new() { NameTask = "Drink beer", Date = "2023-03-20", Of = "10:55", To = "12:55" };
            TimeNote timeNote1 = new() { NameTask = "Drink vodka", Date = "2023-03-20", Of = "13:55", To = "14:55" };
            var user = new ToDoManagerUser() { Email = "testPut@email.test", Password = "123456" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTimeTablesAsync("", timeNote, user.Email);
            await _mockDB.PutTimeTablesAsync("", timeNote1, user.Email);

            var getTimeTables = await _mockDB.GetTimeTablesAsync(user.Email);
            Assert.Equal(timeNote.NameTask, getTimeTables[0].NameTask);
            Assert.Equal(timeNote1.NameTask, getTimeTables[1].NameTask);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }

        [Fact]
        public async void DeleteTimeNote_OK()
        {
            TimeNote timeNote = new() { NameTask = "Drink beer", Date = "2023-03-20", Of = "10:55", To = "12:55" };
            TimeNote timeNote1 = new() { NameTask = "Drink vodka", Date = "2023-03-20", Of = "13:55", To = "14:55" };
            var user = new ToDoManagerUser() { Email = "testPut@email.test", Password = "123456" };
            await _mockDB.CreateUserAsync(user);

            await _mockDB.PutTimeTablesAsync("", timeNote, user.Email);
            await _mockDB.PutTimeTablesAsync("", timeNote1, user.Email);

            await _mockDB.DeleteTimeNoteAsync(timeNote.NameTask,user.Email);
            await _mockDB.DeleteTimeNoteAsync(timeNote1.NameTask,user.Email);
            var getTimeTables = await _mockDB.GetTimeTablesAsync(user.Email);

            Assert.Empty(getTimeTables);
            await _mockDB.DeleteUserForEmailAsync(user.Email);
        }


    }
}
