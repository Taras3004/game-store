using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.DAL.Mongo.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Customer : NorthwindEntity
    {
        [BsonElement("CategoryID")]
        public required string CustomerId { get; set; }
        public required string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Address { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? City { get; set; }
        public string? Region { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? PostalCode { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
    }
}
