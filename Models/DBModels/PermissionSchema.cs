using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace snipetrain_bot.Models
{
    public class PermissionSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("Admin")]
        public string AdminName { get; set; }
        [BsonElement("Date")]
        public DateTimeOffset Date { get; set; }
        [BsonElement("Reason")]
        public string Reason { get; set; }
        [BsonElement("User")]
        public string User { get; set; }
        [BsonElement("UserId")]
        public ulong UserId { get; set; }
    }
}