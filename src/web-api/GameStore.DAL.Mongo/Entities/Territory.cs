using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Territory : NorthwindEntity
    {
        [BsonElement("TerritoryID")]
        public int TerritoryId { get; set; }
        public string? TerritoryDescription { get; set; }

        [BsonElement("RegionID")]
        public int? RegionId { get; set; }
    }
}
