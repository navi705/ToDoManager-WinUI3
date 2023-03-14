using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace ToDoManager.Asp.Net.Core.Models
{
    [BsonIgnoreExtraElements]
    public class MultiTasks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        [BsonElement("email")]
        public string Email { get; set; } = null!;
        
        [BsonElement("tasks")]
        [JsonPropertyName("tasks")]
        public List<NotMultiTask> Tasks { get; set; } = null!;
    }

    public class NotMultiTask
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Time { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Reapet { get; set; } = null!;

        public List<NotMultiTask> Subtasks { get; set; } = null!;

        public bool Auto_fail { get; set; } = false!;

        public List<string> Group { get; set; } = null!;

        public bool Finish { get; set; } = false!;
    }

    
}
