using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.DAL.Mongo.Mappings;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace GameStore.DAL.Mongo
{
    internal class MongoKeyResolver<TDocument>(
            IMemoryCache cache,
            IMongoDatabase database)
            where TDocument : NorthwindEntity
    {
        private readonly IMongoCollection<TDocument> collection =
            database.GetCollection<TDocument>(MongoCollectionMap.GetCollectionName<TDocument>());

        public async Task<string?> GetLegacyIdAsync(Guid domainId)
        {
            var config = MongoEntityConfigMap.GetEntityConfig<TDocument>();

            if(config is null)
            {
                return null;
            }

            string cacheKey = $"MongoMapping_{typeof(TDocument).Name}";

            var mapping = await cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

                var allDocuments = await collection.Find(FilterDefinition<TDocument>.Empty).ToListAsync();

                return allDocuments.ToDictionary(
                    doc => $"{config.Prefix}{config.GetSurrogateKey(doc)}".ToDeterministicGuid(),
                    doc => config.GetSurrogateKey(doc)
                );
            });

            if (mapping != null && mapping.TryGetValue(domainId, out var legacyId))
            {
                return legacyId;
            }

            return null;
        }
    }
}
