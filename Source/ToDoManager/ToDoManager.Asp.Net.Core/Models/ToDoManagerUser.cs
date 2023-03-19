using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoManager.Asp.Net.Core.Models
{
    [BsonIgnoreExtraElements]
    public class ToDoManagerUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        // why null?
        [BsonElement("email")]
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; } = null!;

        [BsonElement("password")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; } = null!;

        [BsonElement("tasks")]
        [JsonPropertyName("tasks")]
        public List<ToDoNote> ToDoTasks { get; set; } = new List<ToDoNote>();

        [BsonElement("timetable")]
        [JsonPropertyName("timetable")]
        public List<TimeNote> TimeTable { get; set; } = new List<TimeNote>();

    }

    public class ToDoNote
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Time { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Reapet { get; set; } = null!;

        public List<ToDoNote> Subtasks { get; set; } = null!;

        public bool Auto_fail { get; set; } = false!;

        public List<string> Group { get; set; } = null!;

        public bool Finish { get; set; } = false!;
    }

    public class TimeNote
    {
        public string Date { get; set; } = null!;

        public string NameTask { get; set; } = null!;

        public string Of { get; set; } = null!;

        public string To { get; set; } = null!;
    }
}
