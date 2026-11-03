using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities.ConfigurationEntities
{
    public abstract class NorthwindEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonExtraElements]
        public IDictionary<string, object>? ExtraElements { get; set; }

    }
}