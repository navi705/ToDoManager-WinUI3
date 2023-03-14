using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoManager.Asp.Net.Core.Models
{
    [BsonIgnoreExtraElements]
    public class ColorModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = null!;
        
        [BsonElement("color")]
        public string Color { get; set; } = null!;
        
    }
}
