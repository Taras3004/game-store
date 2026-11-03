using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Mappings;
using GameStore.DAL.Mongo.Normalization;
using GameStore.Models.Entities;
using GameStore.Models.ValueObjects;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DAL.Mongo.Repositories
{
    internal class MongoGameRepository(
        IMongoDatabase database,
        INorthwindQuerySource<Product> productSource,
        INorthwindQuerySource<Supplier> supplierSource,
        MongoKeyResolver<Product> productKeyResolver,
        MongoKeyResolver<Category> categoryKeyResolver,
        IEntityFilter<Product, GameFilterDetails> productFilter) : IGameRepository
    {
        // Writes go to the raw collection, reads go through the normalized query sources
        private readonly IMongoCollection<Product> _products =
            database.GetCollection<Product>(MongoCollectionMap.GetCollectionName<Product>());

        public async Task<Game?> GetByIdAsync(
            Guid id, GamePermissionDetails? permissions = null, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var legacyId = await productKeyResolver.GetLegacyIdAsync(id);
            if (legacyId is null || !int.TryParse(legacyId, out int productId))
            {
                return null;
            }

            var query = ApplyPermissions(productSource.Query(), permissions);
            var product = await query.FirstOrDefaultAsync(p => p.ProductId == productId, token);

            return product is null ? null : MapToDomain(product);
        }

        public async Task<Game?> GetByKeyAsync(
            string key, GamePermissionDetails? permissions = null, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var query = ApplyPermissions(productSource.Query(), permissions);
            var product = await query.FirstOrDefaultAsync(p => p.ProductKey == key, token);

            return product is null ? null : MapToDomain(product);
        }

        public async Task<IEnumerable<Game>> GetByFilterAsync(
            GameFilterDetails filter, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var predicate = await productFilter.CreateFilter(filter);
            var query = ApplyPermissions(productSource.Query(), permissions);

            var products = await query
                .Where(predicate)
                .ToListAsync(token);

            return products
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductKey))
                .GroupBy(p => p.ProductKey, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public Task<IEnumerable<Game>> GetByPlatformIdAsync(
            Guid platformId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            return Task.FromResult<IEnumerable<Game>>([]);
        }

        public async Task<IEnumerable<Game>> GetByGenreIdAsync(
            Guid genreId, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var legacyCategoryId = await categoryKeyResolver.GetLegacyIdAsync(genreId);
            if (legacyCategoryId is null || !int.TryParse(legacyCategoryId, out int categoryId))
            {
                return [];
            }

            var query = ApplyPermissions(productSource.Query(), permissions);
            var products = await query
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync(token);

            return products
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductKey))
                .GroupBy(p => p.ProductKey, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<IEnumerable<Game>> GetByPublisherCompanyNameAsync(
            string companyName, GamePermissionDetails permissions, CancellationToken token = default)
        {
            var supplier = await supplierSource
                .Query()
                .FirstOrDefaultAsync(s => s.CompanyName.ToLower() == companyName.ToLower(), token);

            if (supplier is null)
            {
                return [];
            }

            var query = ApplyPermissions(productSource.Query(), permissions);
            var products = await query
                .Where(p => p.SupplierId == supplier.SupplierId)
                .ToListAsync(token);

            return products
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductKey))
                .GroupBy(p => p.ProductKey, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<int> GetCountAsync(
            GameFilterDetails? filter = null,
            GamePermissionDetails? permissions = null,
            CancellationToken token = default)
        {
            var query = ApplyPermissions(productSource.Query(), permissions);

            if (filter is not null)
            {
                var predicate = await productFilter.CreateFilter(filter);
                query = query.Where(predicate);
            }

            return await query.CountAsync(token);
        }

        public async Task<bool> IsKeyTakenAsync(
            string key, Guid? excludeId = null, CancellationToken token = default)
        {
            var query = productSource
                .Query()
                .Where(p => p.ProductKey.ToLower() == key.ToLower());

            if (excludeId.HasValue)
            {
                var excludeLegacyId = await productKeyResolver.GetLegacyIdAsync(excludeId.Value);
                if (excludeLegacyId is not null && int.TryParse(excludeLegacyId, out int parsedExcludeId))
                {
                    query = query.Where(p => p.ProductId != parsedExcludeId);
                }
            }

            return await query.AnyAsync(token);
        }

        public Task<Game?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default) =>
            GetByIdAsync(id, permissions: null, token: token);

        public Task<Game?> GetOrMigrateByKeyAsync(string key, CancellationToken token = default) =>
            GetByKeyAsync(key, permissions: null, token: token);

        public async Task IncrementViewsByIdAsync(Guid id, CancellationToken token = default)
        {
            var legacyId = await productKeyResolver.GetLegacyIdAsync(id);
            if (legacyId is null || !int.TryParse(legacyId, out int productId))
            {
                return;
            }

            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            var update = Builders<Product>.Update.Inc(p => p.Views, 1);

            await _products.UpdateOneAsync(filter, update, cancellationToken: token);
        }

        public async Task IncrementViewsByKeyAsync(string key, CancellationToken token = default)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductKey, key);
            var update = Builders<Product>.Update.Inc(p => p.Views, 1);

            await _products.UpdateOneAsync(filter, update, cancellationToken: token);
        }

        public Task IncrementCommentCountAsync(string key, CancellationToken token = default) =>
            Task.CompletedTask;

        public async Task SyncStockAsync(string key, int newStock, CancellationToken token = default)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductKey, key);
            var update = Builders<Product>.Update.Set(p => p.UnitsInStock, newStock);

            await _products.UpdateOneAsync(filter, update, cancellationToken: token);
        }

        public Task AddAsync(Game game, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind products collection does not support direct Game additions.");

        public Task UpdateAsync(Game game, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind products collection is updated via Copy-on-Write to SQL.");

        public Task DeleteAsync(Game game, CancellationToken token = default) => 
            throw new NotSupportedException();

        private static IQueryable<Product> ApplyPermissions(
            IQueryable<Product> query, GamePermissionDetails? permissions)
        {
            if (permissions is not null && !permissions.AccessOutOfStock)
            {
                query = query.Where(p => p.UnitsInStock > 0);
            }

            return query;
        }

        private static Game MapToDomain(Product product)
        {
            var productConfig = MongoEntityConfigMap.GetEntityConfig<Product>();
            var supplierConfig = MongoEntityConfigMap.GetEntityConfig<Supplier>();

            string productSurrogateKey = productConfig.GetSurrogateKey(product);
            Guid gameId = $"{productConfig.Prefix}{productSurrogateKey}".ToDeterministicGuid();
            Guid publisherId = $"{supplierConfig.Prefix}{product.SupplierId}".ToDeterministicGuid();

            var priceResult = GamePrice.Create(product.UnitPrice, 0);

            return new Game(
                product.ProductName,
                product.ProductKey,
                description: string.Empty,
                priceResult.Value,
                product.UnitsInStock,
                publisherId,
                gameId,
                product.Views)
            {
                LegacyId = productSurrogateKey,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitsOnOrder = product.UnitsOnOrder,
            };
        }
    }
}