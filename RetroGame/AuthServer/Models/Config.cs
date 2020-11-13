using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AuthServer.Models
{
    public class Config
    {
        #region Properties
        public string Id { get; set; }

        [BsonElement("configuration")]
        public Dictionary<string, string> Configuration { get; set; }

        #endregion
    }
}