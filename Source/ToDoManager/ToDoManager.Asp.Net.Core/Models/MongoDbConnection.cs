
namespace ToDoManager.Asp.Net.Core.Models
{
    public class MongoDbConnection
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string BooksCollectionName { get; set; } = null!;


    }
}
