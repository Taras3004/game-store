using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Category : NorthwindEntity
    {
        [BsonElement("CategoryID")]
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string Description { get; set; }
        public required string Picture { get; set; }
    }
}