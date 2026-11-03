using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.DAL.Mongo.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Supplier : NorthwindEntity
    {
        [BsonElement("SupplierID")]
        public int SupplierId { get; set; }
        public required  string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Address { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? City { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Region { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? PostalCode { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Country { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Phone { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? Fax { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? HomePage { get; set; }
    }
}
