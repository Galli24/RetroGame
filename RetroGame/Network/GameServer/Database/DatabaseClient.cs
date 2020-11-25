using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Database
{
    class DatabaseClient
    {
        #region Members

        private readonly MongoClient _client;
        private string _headerKey;

        private class Config
        {
            public string Id { get; set; }

            [BsonElement("configuration")]
            public Dictionary<string, string> Configuration { get; set; }
        }

        #endregion

        #region Constructor

        public DatabaseClient(string connectionString)
        {
            _client = new MongoClient(connectionString);
            Console.WriteLine("Database client ready");
        }

        #endregion

        #region Header Key

        public string GetHeaderKey()
        {
            if (!string.IsNullOrEmpty(_headerKey))
                return _headerKey;

            var database = _client.GetDatabase("Auth");
            var configurations = database.GetCollection<Config>("Configuration");
            var apiConfig = configurations.Find(configuration => configuration.Id == "auth_configuration").FirstOrDefault();

            if (apiConfig.Configuration.ContainsKey("header_key"))
            {
                _headerKey = apiConfig.Configuration["header_key"];
                return _headerKey;
            }

            return string.Empty;
        }

        public string ResetAndFetchHeaderKey()
        {
            _headerKey = string.Empty;
            return GetHeaderKey();
        }

        #endregion
    }
}
