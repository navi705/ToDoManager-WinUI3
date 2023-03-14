using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoManager.Asp.Net.Core.Models
{
    [BsonIgnoreExtraElements]
    public class TimeTable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("timetable")]
        [JsonPropertyName("timetable")]
        public List<Time> TimeTables { get; set; } = null!;
    }

    public class Time
    {
        public string Date { get; set; } = null!;

        public string NameTask { get; set; } = null!;

        public string Of { get; set; } = null!;

        public string To { get; set; } = null!;

    }
}
