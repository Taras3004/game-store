using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.DAL.Mongo.Mappings;
using MongoDB.Driver;

namespace GameStore.DAL.Mongo.Normalization
{
    internal class NorthwindQuerySource<TDocument>(IMongoDatabase database) : INorthwindQuerySource<TDocument>
        where TDocument : NorthwindEntity
    {
        private readonly IMongoCollection<TDocument> _collection =
            database.GetCollection<TDocument>(MongoCollectionMap.GetCollectionName<TDocument>());

        public IQueryable<TDocument> Query() => _collection.AsQueryable();
    }
}
