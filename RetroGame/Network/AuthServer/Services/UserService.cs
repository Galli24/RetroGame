using AuthServer.Dtos;
using AuthServer.Exceptions;
using AuthServer.Models;
using AuthServer.PasswordHasher;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace AuthServer.Services
{
    public interface IUserService
    {
        User Authenticate(UserDto userDto);
        User Create(User user, string password);
        List<User> GetAll();
        User GetUserById(string id);
        User GetUserByUsername(string username);

        void Update(string id, User user);
        void Remove(User user);
        void Remove(string id);
    }

    public class UserService : IUserService
    {
        #region Members

        private readonly IMongoCollection<User> _users;
        private readonly BCryptPasswordHasher<User> _passwordHasher;

        #endregion

        #region Constructor

        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("DB"));
            var database = client.GetDatabase("Auth");
            _users = database.GetCollection<User>("Users");
            _passwordHasher = new BCryptPasswordHasher<User>();
        }

        #endregion

        #region Auth

        public User Authenticate(UserDto userDto)
        {
            var user = GetUserByUsername(userDto.Username);
            if (user == null)
                return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password);
            if (verificationResult == PasswordVerificationResult.Success)
                return user;

            return null;
        }

        #endregion

        #region Logic

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new RegisterException("Email can't be empty");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new RegisterException("Username can't be empty");

            if (string.IsNullOrWhiteSpace(password))
                throw new RegisterException("Password can't be empty");

            if (_users.Find(duplicateUser => duplicateUser.Email == user.Email).FirstOrDefault() != null)
                throw new RegisterException("Email already used");

            if (_users.Find(duplicateUser => duplicateUser.Username == user.Username).FirstOrDefault() != null)
                throw new RegisterException("Username already used");

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _users.InsertOne(user);
            return user;
        }

        public List<User> GetAll() => _users.Find(user => true).ToList();

        public User GetUserById(string id)
        {
            var docId = new ObjectId(id);

            return _users.Find(user => user.Id == docId).FirstOrDefault();
        }

        public User GetUserByUsername(string username) => _users.Find(user => user.Username == username).FirstOrDefault();

        public void Update(string id, User userIn)
        {
            var docId = new ObjectId(id);

            _users.ReplaceOne(user => user.Id == docId, userIn);
        }

        public void Remove(User userIn) => _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id)
        {
            var docId = new ObjectId(id);

            _users.DeleteOne(user => user.Id == docId);
        }

        #endregion

    }
}