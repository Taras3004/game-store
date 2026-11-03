using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    public class Log : NorthwindEntity
    {
        public DateTime? Date { get; set; }
        public string? Action { get; set; }
        public string? EntityType { get; set; }

        [BsonIgnoreIfNull]
        public string? OldVersion { get; set; }

        [BsonIgnoreIfNull]
        public string? NewVersion { get; set; }
    }
}
