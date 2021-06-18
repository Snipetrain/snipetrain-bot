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
        public DateTime CreatedDate { get; set; }

        [BsonElement("ExpiryDate")]
        public DateTime ExpiryDate { get; set; }
        [BsonElement("InitiatedBy")]
        public string InitiatedBy { get; set; }
        [BsonElement("Region")]
        public string Region { get; set; }
        [BsonElement("State")]
        public PartyState State { get; set; }
        [BsonElement("MessageId")]
        public ulong MessageId { get; set; }
    }


    public enum PartyState
    {
        Inactive = 0,
        Voting = 1,
        Completed = 2
    }
}