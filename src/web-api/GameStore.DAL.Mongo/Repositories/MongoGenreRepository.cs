using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Mappings;
using GameStore.DAL.Mongo.Normalization;
using GameStore.Models.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DAL.Mongo.Repositories
{
    internal class MongoGenreRepository(
        INorthwindQuerySource<Category> categorySource,
        INorthwindQuerySource<Product> productSource,
        MongoKeyResolver<Category> keyResolver) : IGenreRepository
    {
        public async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken token = default)
        {
            var categories = await categorySource
                .Query()
                .ToListAsync(token);

            // E08 US6: якщо в MongoDB є два однакові елементи — використовується перший знайдений
            return categories
                .Where(c => !string.IsNullOrWhiteSpace(c.CategoryName))
                .GroupBy(c => c.CategoryName, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<Genre?> GetByNameAsync(
            string name, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var category = await categorySource
                .Query()
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower(), token);

            return category is null ? null : MapToDomain(category);
        }

        public async Task<Genre?> GetByIdAsync(
            Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var legacyId = await keyResolver.GetLegacyIdAsync(id);
            if (legacyId is null || !int.TryParse(legacyId, out int categoryId))
            {
                return null;
            }

            var category = await categorySource
                .Query()
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId, token);

            return category is null ? null : MapToDomain(category);
        }

        public Task<IEnumerable<Genre>> GetByParentIdAsync(Guid parentId, CancellationToken token = default)
        {
            // Категорії в Northwind не мають ієрархії. Якщо категорії призначається батьківський жанр,
            // вона мігрує в SQL-базу (E08 US7), тому в Mongo дочірніх жанрів бути не може.
            return Task.FromResult<IEnumerable<Genre>>([]);
        }

        public async Task<IEnumerable<Genre>> GetByGameKeyAsync(string gameKey, CancellationToken token = default)
        {
            var product = await productSource
                .Query()
                .FirstOrDefaultAsync(p => p.ProductKey == gameKey, token);

            if (product is null)
            {
                return [];
            }

            var categories = await categorySource
                .Query()
                .Where(c => c.CategoryId == product.CategoryId)
                .ToListAsync(token);

            return categories
                .GroupBy(c => c.CategoryName, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<List<Genre>> GetOrMigrateByIdsAsync(IEnumerable<Guid> ids, CancellationToken token = default)
        {
            var categoryIds = new List<int>();

            foreach (var id in ids.Distinct())
            {
                var legacyId = await keyResolver.GetLegacyIdAsync(id);
                if (legacyId is not null && int.TryParse(legacyId, out int parsedId))
                {
                    categoryIds.Add(parsedId);
                }
            }

            if (categoryIds.Count == 0)
            {
                return [];
            }

            var categories = await categorySource
                .Query()
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync(token);

            return categories
                .GroupBy(c => c.CategoryId)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId = null, CancellationToken token = default)
        {
            var query = categorySource
                .Query()
                .Where(c => c.CategoryName.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                var excludeLegacyId = await keyResolver.GetLegacyIdAsync(excludeId.Value);
                if (excludeLegacyId is not null && int.TryParse(excludeLegacyId, out int parsedExcludeId))
                {
                    query = query.Where(c => c.CategoryId != parsedExcludeId);
                }
            }

            return await query.AnyAsync(token);
        }

        public Task AddAsync(Genre genre, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind categories collection is read-only.");

        public Task UpdateAsync(Genre genre, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind categories collection is read-only.");

        public Task DeleteAsync(Genre genre, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind categories collection is read-only.");

        private static Genre MapToDomain(Category category)
        {
            var config = MongoEntityConfigMap.GetEntityConfig<Category>();
            string surrogateKey = config.GetSurrogateKey(category);
            Guid deterministicId = $"{config.Prefix}{surrogateKey}".ToDeterministicGuid();

            return new Genre(category.CategoryName)
            {
                Id = deterministicId,
                LegacyId = surrogateKey,
                Description = category.Description,
                Picture = category.Picture
            };
        }
    }
}