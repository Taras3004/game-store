using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.DAL.Mongo.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Order : NorthwindEntity
    {
        [BsonElement("OrderID")]
        public int OrderId { get; set; }

        [BsonElement("CustomerID")]
        public string? CustomerId { get; set; }

        [BsonElement("EmployeeID")]
        public int? EmployeeId { get; set; }

        [BsonSerializer(typeof(DateTimeSerializer))]
        public DateTime? OrderDate { get; set; }

        [BsonSerializer(typeof(DateTimeSerializer))]
        public DateTime? RequiredDate { get; set; }

        [BsonSerializer(typeof(DateTimeSerializer))]
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public double? Freight { get; set; }
        public string? ShipName { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? ShipAddress { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? ShipPostalCode { get; set; }

        [BsonSerializer(typeof(IntToStringSerializer))]
        public string? ShipCountry { get; set; }
    }
}
