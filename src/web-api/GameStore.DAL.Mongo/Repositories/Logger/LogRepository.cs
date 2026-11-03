using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Mongo.Repositories.Logger
{
    internal class LogRepository(IMongoDatabase database) : ILogRepository
    {
        protected readonly IMongoCollection<Log> Collection =
            database.GetCollection<Log>(MongoCollectionMap.GetCollectionName<Product>());

        public async Task Add(Log log, CancellationToken token)
        {
            var collectionName = Collection.CollectionNamespace.CollectionName;

            var command = new BsonDocument
            {
                { "insert", collectionName },
                { "documents", new BsonArray { log.ToBsonDocument() } }
            };

            await database.RunCommandAsync<BsonDocument>(command, cancellationToken: token);
        }

    }
}
