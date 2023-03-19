using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoManager.Asp.Net.Core.Models;

namespace ToDoManager.Asp.Net.Core.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<ToDoManagerUser> _userCollection;

        public MongoDbService(IOptions<MongoDbConnection> mongoDbConnection)
        {
            MongoClient mongoClient = new(mongoDbConnection.Value.ConnectionString);

            IMongoDatabase database = mongoClient.GetDatabase(mongoDbConnection.Value.DatabaseName);

            _userCollection = database.GetCollection<ToDoManagerUser>(mongoDbConnection.Value.BooksCollectionName);
        }

        public async Task<bool> CreateUserAsync(ToDoManagerUser user)
        {
            // найти способ типо миграции чтобы перенести индекс и создание коллекций при первой инцилизации базы данных и для тестов ci/di containers
            //_usersCollection.Indexes.CreateOne(
            //    new CreateIndexModel<ToDoManagerUser>(
            // Builders<ToDoManagerUser>.IndexKeys.Ascending(d => d.Email),
            //new CreateIndexOptions { Unique = true }
            //));
            try
            {
                await _userCollection.InsertOneAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ToDoManagerUser> GetUserForEmailAsync(string email)
        {
            FilterDefinition<ToDoManagerUser> filter = Builders<ToDoManagerUser>.Filter.Eq("email", email);
            var answerDB = await _userCollection.Find(filter).ToListAsync();
            if (answerDB.Count == 0)
                return new ToDoManagerUser();
            return answerDB[0];
        }

        public async Task<List<ToDoNote>> GetTasksAsync(string email)
        {
            ToDoManagerUser user = await GetUserForEmailAsync(email);
            return user.ToDoTasks;
        }

        public async Task PutTaskAsync(string nameTask, ToDoNote toDoNote, string email)
        {
            var filter = Builders<ToDoManagerUser>.Filter.Eq(x => x.Email, email) & 
            Builders<ToDoManagerUser>.Filter.ElemMatch(x => x.ToDoTasks, Builders<ToDoNote>.Filter.Eq(x => x.Name, nameTask));

            UpdateDefinition<ToDoManagerUser> updateTask = Builders<ToDoManagerUser>.Update.Set(x => x.ToDoTasks[-1], toDoNote);

            var aboba = await _userCollection.UpdateOneAsync(filter, updateTask);
            if (aboba.MatchedCount <= 0)
            {
                var filter1 = new BsonDocument { { "email", email } };

                UpdateDefinition<ToDoManagerUser> updateTask1 = Builders<ToDoManagerUser>.Update.Push<ToDoNote>("tasks", toDoNote);

                await _userCollection.UpdateOneAsync(filter1, updateTask1);
            }
        }

        public async Task DeleteTaskAsync(string nameTask, string email)
        {
            var filter = Builders<ToDoManagerUser>.Filter.Eq(x => x.Email, email) & Builders<ToDoManagerUser>.Filter.ElemMatch(x => x.ToDoTasks, Builders<ToDoNote>.Filter.Eq(x => x.Name, nameTask));

            UpdateDefinition<ToDoManagerUser> updateTask = Builders<ToDoManagerUser>.Update.PullFilter<ToDoNote>(x => x.ToDoTasks, Builders<ToDoNote>.Filter.Eq(x => x.Name, nameTask));

            await _userCollection.UpdateOneAsync(filter, updateTask);
        }

        public async Task<List<TimeNote>> GetTimeTablesAsync(string email)
        {
            ToDoManagerUser user = await GetUserForEmailAsync(email);
            return user.TimeTable;
        }

        public async Task PutTimeTablesAsync(string name, TimeNote time,string email)
        {
            var filter = Builders<ToDoManagerUser>.Filter.Eq(x => x.Email, email) & Builders<ToDoManagerUser>.Filter.ElemMatch(x => x.TimeTable, Builders<TimeNote>.Filter.Eq(x => x.NameTask, name));

            UpdateDefinition<ToDoManagerUser> updatetime = Builders<ToDoManagerUser>.Update.Set<TimeNote>(x => x.TimeTable[-1], time);

            var aboba = await _userCollection.UpdateOneAsync(filter, updatetime);
            if (aboba.MatchedCount <= 0)
            {
                var filter1 = new BsonDocument { { "email", email } };


                UpdateDefinition<ToDoManagerUser> updatetime1 = Builders<ToDoManagerUser>.Update.Push<TimeNote>("timetable", time);

                await _userCollection.UpdateOneAsync(filter1, updatetime1);
            }
        }

        public async Task DeleteTimeNoteAsync(string name,string email)
        {
            var filter = Builders<ToDoManagerUser>.Filter.Eq(x => x.Email, email) & Builders<ToDoManagerUser>.Filter.ElemMatch(x => x.TimeTable, Builders<TimeNote>.Filter.Eq(x => x.NameTask, name));

            UpdateDefinition<ToDoManagerUser> updateTask = Builders<ToDoManagerUser>.Update.PullFilter<TimeNote>(x => x.TimeTable, Builders<TimeNote>.Filter.Eq(x => x.NameTask, name));

            var aboba = await _userCollection.UpdateOneAsync(filter, updateTask);
        }

        public async Task DeleteUserForEmailAsync(string email)
        {
            FilterDefinition<ToDoManagerUser> filter = Builders<ToDoManagerUser>.Filter.Eq("email", email);
            await _userCollection.DeleteOneAsync(filter);
        }
    }
}
