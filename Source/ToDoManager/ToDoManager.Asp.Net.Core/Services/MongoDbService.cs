using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel;
using System.Threading.Tasks;
using ToDoManager.Asp.Net.Core.Models;

namespace ToDoManager.Asp.Net.Core.Services
{
    public class MongoDbService
    {
        //private
        public readonly IMongoCollection<User> _userCollection;

        //public readonly IMongoCollection<BsonDocument> collection;

        public readonly IMongoCollection<MultiTasks> _multiTasksCollection;

        public readonly IMongoCollection<TimeTable> _timeTableCollection;

        public readonly IMongoCollection<ColorModel> _colorCollection;

        public string emailn { get; set; } = null!;

        public MongoDbService(IOptions<MongoDbConnection> mongoDbConnection)
        {
            MongoClient mongoClient = new MongoClient(mongoDbConnection.Value.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(mongoDbConnection.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDbConnection.Value.BooksCollectionName);
            
            //collection = database.GetCollection<BsonDocument>(mongoDbConnection.Value.BooksCollectionName);

            _multiTasksCollection = database.GetCollection<MultiTasks>(mongoDbConnection.Value.BooksCollectionName);

            _timeTableCollection = database.GetCollection<TimeTable>(mongoDbConnection.Value.BooksCollectionName);
            
            _colorCollection = database.GetCollection<ColorModel>(mongoDbConnection.Value.BooksCollectionName);
        }

        public async Task CreateAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            return;
        }


        public async Task<List<User>> GetAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<User>> GetAsyncEmail(string email)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", email);
            return await _userCollection.Find(filter).ToListAsync();
        }


        public async Task AddTokenAsync(string email, string token, string device)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", email);
            UpdateDefinition<User> updateDevice = Builders<User>.Update.AddToSet<string>("tokens",device);
            UpdateDefinition<User> updateToken = Builders<User>.Update.AddToSet<string>("tokens", token);

            List<User> user = await GetAsyncEmail(email);
            
            if (user[0].Tokens.IndexOf(device) == -1)
            {
                await _userCollection.UpdateOneAsync(filter, updateDevice);
                await _userCollection.UpdateOneAsync(filter, updateToken);
                return;
            }
            else
            {
                int index = user[0].Tokens.IndexOf(device);
                user[0].Tokens[index+1] = token;
                UpdateDefinition<User> updateToken1 = Builders<User>.Update.Set<List<string>>("tokens", user[0].Tokens);
                await _userCollection.UpdateOneAsync(filter, updateToken1);
            }
                
            return;
        }

        public async Task AddTaskAsync(NotMultiTask task)
        {
            var filter = new BsonDocument { { "email", emailn } };
            
            
            UpdateDefinition<MultiTasks> updateTask = Builders<MultiTasks>.Update.Push<NotMultiTask>("tasks", task);

            await _multiTasksCollection.UpdateOneAsync(filter, updateTask);
            
        }

        public async Task AddTaskDefautAsync(string email)
        {
            var filter = new BsonDocument { { "email", email } };
            List<NotMultiTask> defauls = new List<NotMultiTask>();
            NotMultiTask a = new NotMultiTask {
                Name = "Заправить постель",
                Description = "Да",
                Time = "8:00",
                Date = "12.12",
                Reapet = "everyeday",
                Subtasks = new List<NotMultiTask>(),
                Auto_fail = true,
                Group = new List<string>(),
                Finish = false

            };
            defauls.Add(a);
            UpdateDefinition<MultiTasks> updateTask = Builders<MultiTasks>.Update.Set<List<NotMultiTask>>("tasks", defauls);

            await _multiTasksCollection.UpdateOneAsync(filter, updateTask);
            return;
            //FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", email);
            //return await _userCollection.Find(filter).ToListAsync();
        }
        
        public async Task<MultiTasks> GetTasksAsync()
        {
            var filter = new BsonDocument { { "email", emailn } };
            //List<BsonDocument> answer = await collection.Find(filter).ToListAsync();
            //List<string> aboba = Convert.(answer[0].Elements);           
            List<MultiTasks> answer1 = await _multiTasksCollection.Find(filter).ToListAsync();
            return answer1[0];
            //if (answer1[0].Tasks == null)
            //{
            //   //await AddTaskDefautAsync(emailn);
            //    return await _multiTasksCollection.Find(filter).ToListAsync();
            //}             
            //return answer1;
            //FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", email);
            //return await _userCollection.Find(filter).ToListAsync();
        }

        public async Task PutTaskAsync(string nameTask, NotMultiTask task )
        {
            var filter = Builders<MultiTasks>.Filter.Eq(x => x.Email, emailn) & Builders<MultiTasks>.Filter.ElemMatch(x => x.Tasks, Builders<NotMultiTask>.Filter.Eq(x => x.Name, nameTask));

            UpdateDefinition<MultiTasks> updateTask = Builders<MultiTasks>.Update.Set<NotMultiTask>(x => x.Tasks[-1], task);

            var aboba  = await _multiTasksCollection.UpdateOneAsync(filter,updateTask);
            if(aboba.MatchedCount <= 0)
            {
                var filter1 = new BsonDocument { { "email", emailn } };


                UpdateDefinition<MultiTasks> updateTask1 = Builders<MultiTasks>.Update.Push<NotMultiTask>("tasks", task);

                await _multiTasksCollection.UpdateOneAsync(filter1, updateTask1);
            }
               return; 
        }

        public async Task DeleteTaskAsync(string nameTask)
        {
            var filter = Builders<MultiTasks>.Filter.Eq(x => x.Email, emailn) & Builders<MultiTasks>.Filter.ElemMatch(x => x.Tasks, Builders<NotMultiTask>.Filter.Eq(x => x.Name, nameTask));

            UpdateDefinition<MultiTasks> updateTask = Builders<MultiTasks>.Update.PullFilter<NotMultiTask>(x => x.Tasks, Builders<NotMultiTask>.Filter.Eq(x => x.Name, nameTask));

            var aboba = await _multiTasksCollection.UpdateOneAsync(filter, updateTask);
            return;     
        }
        
        public async Task AddTimeTableAsync(Time time)
        {
            var filter = new BsonDocument { { "email", emailn } };


            UpdateDefinition<TimeTable> updateTask = Builders<TimeTable>.Update.Push<Time>("timetable", time);

            await _timeTableCollection.UpdateOneAsync(filter, updateTask);

        }
        //убрать лист как и в  GetTasksAsync()
        public async Task<TimeTable> GetTimeTablesAsync()
        {
            var filter = new BsonDocument { { "email", emailn } };     
            List<TimeTable> answer1 = await _timeTableCollection.Find(filter).ToListAsync();        
            return answer1[0];
        }
        
        public async Task PutTimeTableAsync(string name, Time time)
        {
            //var filter = Builders<TimeTable>.Filter.Eq(x => x.Email, emailn) & Builders<TimeTable>.Filter.ElemMatch(x => x.TimeTables, Builders<Time>.Filter.Eq(x => x.Date, date));

            //UpdateDefinition<TimeTable> updateTask = Builders<TimeTable>.Update.Set<Time>(x => x.TimeTables[-1], time);

            //var aboba = await _timeTableCollection.UpdateOneAsync(filter, updateTask);
            //return;
            //var filter = Builders<TimeTable>.Filter.Eq(x => x.Email, emailn) & Builders<TimeTable>.Filter.ElemMatch(x => x.TimeTables, Builders<Time>.Filter.Eq(x => x.Date, date));

            //UpdateDefinition<TimeTable> updateTask = Builders<TimeTable>.Update.Set<Time>(x => x.TimeTables[-1], time);

            //var aboba = await _timeTableCollection.UpdateOneAsync(filter, updateTask);
            //if (aboba.MatchedCount <= 0)
            //{
            //    var filter1 = new BsonDocument { { "email", emailn } };


            //    UpdateDefinition<TimeTable> updateTask1 = Builders<TimeTable>.Update.Push<Time>("timetable", time);

            //    await _timeTableCollection.UpdateOneAsync(filter1, updateTask1);
            //}
            //return;
            var filter = Builders<TimeTable>.Filter.Eq(x => x.Email, emailn) & Builders<TimeTable>.Filter.ElemMatch(x => x.TimeTables, Builders<Time>.Filter.Eq(x => x.NameTask, name));

            UpdateDefinition<TimeTable> updatetime = Builders<TimeTable>.Update.Set<Time>(x => x.TimeTables[-1], time);

            var aboba = await _timeTableCollection.UpdateOneAsync(filter, updatetime);
            if (aboba.MatchedCount <= 0)
            {
                var filter1 = new BsonDocument { { "email", emailn } };


                UpdateDefinition<TimeTable> updatetime1 = Builders<TimeTable>.Update.Push<Time>("timetable", time);

                await _timeTableCollection.UpdateOneAsync(filter1, updatetime1);
            }
            return;

        }

        public async Task DeleteTimeTableAsync(string name)
        {
            var filter = Builders<TimeTable>.Filter.Eq(x => x.Email, emailn) & Builders<TimeTable>.Filter.ElemMatch(x => x.TimeTables, Builders<Time>.Filter.Eq(x => x.NameTask, name));

            UpdateDefinition<TimeTable> updateTask = Builders<TimeTable>.Update.PullFilter<Time>(x => x.TimeTables, Builders<Time>.Filter.Eq(x => x.NameTask, name));

            var aboba = await _timeTableCollection.UpdateOneAsync(filter, updateTask);
            return;
        }

        public async Task DeleteAsync (string id)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
            await _userCollection.DeleteOneAsync(filter);
            return;
        }
        
        public async Task UpdateColorAsync(string color)
        {
            var filter = new BsonDocument { { "email", emailn } };
            UpdateDefinition<ColorModel> update = Builders<ColorModel>.Update.Set<string>("color", color);
            
            await _colorCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task<string> GetColorAsync()
        {
            var filter = new BsonDocument { { "email", emailn } };
            ColorModel aboaba =   _colorCollection.Find(filter).First();
            return aboaba.Color;
        } 

    }
}
