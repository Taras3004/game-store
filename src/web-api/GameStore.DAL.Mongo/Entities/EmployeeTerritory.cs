using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class EmployeeTerritory : NorthwindEntity
    {
        [BsonElement("EmployeeID")]
        public int? EmployeeId { get; set; }

        [BsonElement("TerritoryID")]
        public int? TerritoryId { get; set; }
    }
}
