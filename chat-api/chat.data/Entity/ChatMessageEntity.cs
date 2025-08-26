using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Data.Entity
{
    public class ChatMessageEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("sender")]
        public string Sender { get; set; }

        [BsonElement("receiver")]
        public string Receiver { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
