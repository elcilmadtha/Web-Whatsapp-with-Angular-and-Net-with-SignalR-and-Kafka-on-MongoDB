using MongoDB.Bson.Serialization.Attributes;

namespace Chat.Data.Models
{
    public class UserLoginModel
    {
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
