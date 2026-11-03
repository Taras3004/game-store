using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using System.Collections.Frozen;

namespace GameStore.DAL.Mongo.Mappings
{
    internal static class MongoEntityConfigMap
    {
        private static readonly FrozenDictionary<Type, EntityMappingConfig> Configs =
            new Dictionary<Type, EntityMappingConfig>
            {
                { typeof(Category), new("category-", x => ((Category)x).CategoryId.ToString()) },
                { typeof(Customer), new("customer-", x => ((Customer)x).CustomerId) },
                { typeof(Employee), new("employee-", x => ((Employee)x).EmployeeId.ToString()) },
                { typeof(Order), new("orders-", x => ((Order)x).OrderId.ToString()) },
                { typeof(Product), new("product-", x => ((Product)x).ProductId.ToString()) },
                { typeof(Region), new("region-", x => ((Region)x).RegionId.ToString()) },
                { typeof(Shipper), new("shipper-", x => ((Shipper)x).ShipperId.ToString()) },
                { typeof(Supplier), new("supplier-", x => ((Supplier)x).SupplierId.ToString()) },
                { typeof(Territory), new("territory-", x => ((Territory)x).TerritoryId.ToString()) },

            }.ToFrozenDictionary();

        public static EntityMappingConfig GetEntityConfig<T>()
               where T : NorthwindEntity
        {
            if (Configs.TryGetValue(typeof(T), out var config))
            {
                return config;
            }

            throw new NotSupportedException();
        }
    }
}
