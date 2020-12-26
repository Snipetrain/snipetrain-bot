using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Microsoft.Extensions.Configuration;
using System;

namespace snipetrain_bot.Models
{
    public class EventSchema
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("admin_name")]
        public string Name { get; set; }
        [BsonElement("AnDate")]
        public string AnDate { get; set; }
        [BsonElement("Prize")]
        public string Prize { get; set; }
        [BsonElement("Message")]
        public string Message { get; set; }
        [BsonElement("EventDay")]
        public DateTime EventDay { get; set; }

    }
}