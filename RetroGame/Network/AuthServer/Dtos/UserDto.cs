using MongoDB.Bson;

namespace AuthServer.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}