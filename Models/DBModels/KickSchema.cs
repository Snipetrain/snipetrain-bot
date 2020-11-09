using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace snipetrain_bot.Models
{
    public class KickSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("Admin")]
        public string adminName { get; set; }
        [BsonElement("Date")]
        public string Date { get; set; }
        [BsonElement("Reason")]
        public string Reason { get; set; }
    }
}