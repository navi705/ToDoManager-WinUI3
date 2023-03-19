using Microsoft.Extensions.Options;
using ToDoManager.Asp.Net.Core.Models;

namespace ToDoManager.Tests
{
    static class GlobalVariablesForTests
    {
        public readonly static MongoDbConnection MongoDbCon = new() { ConnectionString = "mongodb://localhost:27017",
        DatabaseName = "TaskManager", BooksCollectionName = "TestUser" };
        public readonly static JwtOption jwtOption = new() { Issuer = "aboba", Audience= "aboba1", Secret= "sdg23gdSdsgfasfg675adDgdfg5sdASDfgsd678S767aboba"
            , TokenLifeTime = 120 };
        public readonly static IOptions<JwtOption> OptionJwt = Options.Create<JwtOption>(jwtOption);
        public readonly static IOptions<MongoDbConnection> OptionMongoDbCon = Options.Create<MongoDbConnection>(MongoDbCon);
    }
}
