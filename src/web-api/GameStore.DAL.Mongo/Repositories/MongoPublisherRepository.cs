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
    internal class MongoPublisherRepository(
        INorthwindQuerySource<Supplier> supplierSource,
        INorthwindQuerySource<Product> productSource,
        MongoKeyResolver<Supplier> keyResolver) : IPublisherRepository
    {
        public async Task<Publisher?> GetByIdAsync(Guid id, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var legacyId = await keyResolver.GetLegacyIdAsync(id);
            if (legacyId is null || !int.TryParse(legacyId, out int supplierId))
            {
                return null;
            }

            var supplier = await supplierSource
                .Query()
                .FirstOrDefaultAsync(s => s.SupplierId == supplierId, token);

            return supplier is null ? null : MapToDomain(supplier);
        }

        public async Task<Publisher?> GetByCompanyNameAsync(string companyName, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var supplier = await supplierSource
                .Query()
                .FirstOrDefaultAsync(s => s.CompanyName.ToLower() == companyName.ToLower(), token);

            return supplier is null ? null : MapToDomain(supplier);
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync(CancellationToken token = default)
        {
            var suppliers = await supplierSource
                .Query()
                .ToListAsync(token);

            return suppliers
                .Where(s => !string.IsNullOrWhiteSpace(s.CompanyName))
                .GroupBy(s => s.CompanyName, StringComparer.OrdinalIgnoreCase)
                .Select(g => MapToDomain(g.First()))
                .ToList();
        }

        public async Task<Publisher?> GetByGameKeyAsync(string gameKey, TrackingBehaviour tracking = TrackingBehaviour.NoTracking, CancellationToken token = default)
        {
            var product = await productSource
                .Query()
                .FirstOrDefaultAsync(p => p.ProductKey == gameKey, token);

            if (product?.SupplierId is null)
            {
                return null;
            }

            var supplier = await supplierSource
                .Query()
                .FirstOrDefaultAsync(s => s.SupplierId == product.SupplierId, token);

            return supplier is null ? null : MapToDomain(supplier);
        }

        public Task<Publisher?> GetOrMigrateByIdAsync(Guid id, CancellationToken token = default)
        {
            return GetByIdAsync(id, token: token);
        }

        public async Task<bool> IsNameTakenAsync(string companyName, CancellationToken token = default)
        {
            return await supplierSource
                .Query()
                .AnyAsync(s => s.CompanyName.ToLower() == companyName.ToLower(), token);
        }

        public async Task<bool> IsNameTakenExcludeIdAsync(string companyName, Guid excludeId, CancellationToken token = default)
        {
            var query = supplierSource
                .Query()
                .Where(s => s.CompanyName.ToLower() == companyName.ToLower());

            var excludeLegacyId = await keyResolver.GetLegacyIdAsync(excludeId);
            if (excludeLegacyId is not null && int.TryParse(excludeLegacyId, out int parsedExcludeId))
            {
                query = query.Where(s => s.SupplierId != parsedExcludeId);
            }

            return await query.AnyAsync(token);
        }

        public Task AddAsync(Publisher publisher, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind suppliers collection is read-only.");

        public Task UpdateAsync(Publisher publisher, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind suppliers collection is read-only.");

        public Task DeleteAsync(Publisher publisher, CancellationToken token = default) =>
            throw new NotSupportedException("Northwind suppliers collection is read-only.");

        private static Publisher MapToDomain(Supplier supplier)
        {
            var config = MongoEntityConfigMap.GetEntityConfig<Supplier>();
            string surrogateKey = config.GetSurrogateKey(supplier);
            Guid deterministicId = $"{config.Prefix}{surrogateKey}".ToDeterministicGuid();

            return new Publisher(supplier.CompanyName, supplier.HomePage, null, deterministicId)
            {
                LegacyId = surrogateKey,
                Address = supplier.Address,
                City = supplier.City,
                Region = supplier.Region,
                PostalCode = supplier.PostalCode,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Fax = supplier.Fax,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle
            };
        }
    }
}