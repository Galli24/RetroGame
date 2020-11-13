using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthServer.Models
{
    public class User
    {
        public ObjectId Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }
    }
}