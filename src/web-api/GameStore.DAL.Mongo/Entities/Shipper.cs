using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Shipper : NorthwindEntity
    {
        [BsonElement("ShipperID")]
        public int ShipperId { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    }
}
