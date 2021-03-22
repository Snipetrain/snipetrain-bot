using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace snipetrain_bot.Models
{
    public class PartySchema
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("CreatedDate")]
        public DateTimeOffset CreatedDate { get; set; }
        [BsonElement("CreatedDate")]
        public DateTimeOffset ExpiryDate { get; set; }
        [BsonElement("InitiatedBy")]
        public string InitiatedBy { get; set; }
        [BsonElement("Region")]
        public string Region { get; set; }
        [BsonElement("State")]
        public PartyState State { get; set; }
    }


    public enum PartyState
    {
        INACTIVE = 0,
        VOTING = 1,
        COMPLETED = 2
    }
}