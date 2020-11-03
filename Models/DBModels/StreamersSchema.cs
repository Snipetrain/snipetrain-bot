using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace snipetrain_bot.Models
{   
    public class StreamersSchema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("twitch_user_id")]
        public string TwitchUserId { get; set; }
    }
}