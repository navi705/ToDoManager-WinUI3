using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace ToDoManager.Asp.Net.Core.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("email")]
        public string Email { get; set; } = null!;
        [BsonElement("password")]
        public string Password { get; set; } = null!;

        [BsonElement("tokens")]
        [JsonPropertyName("tokens")]
        public List<string> Tokens { get; set; } = null!;
    }
}
