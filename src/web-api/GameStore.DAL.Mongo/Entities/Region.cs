using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Region : NorthwindEntity
    {
        [BsonElement("RegionID")]
        public int RegionId { get; set; }
        public string? RegionDescription { get; set; }
    }
}
