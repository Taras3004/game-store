using GameStore.DAL.Mongo.Entities;
using System.Collections.Frozen;

namespace GameStore.DAL.Mongo.Mappings
{
    internal static class MongoCollectionMap
    {
        private static FrozenDictionary<Type, string> CollectionNames { get; } = new Dictionary<Type, string>
        {
            { typeof(Category), "categories" },
            { typeof(Customer), "customers" },
            { typeof(EmployeeTerritory), "employee-territories" },
            { typeof(Employee), "employees" },
            { typeof(OrderDetails), "order-details" },
            { typeof(Order), "orders" },
            { typeof(Product), "products" },
            { typeof(Region), "regions" },
            { typeof(Shipper), "shippers" },
            { typeof(Supplier), "suppliers" },
            { typeof(Territory), "territories" },
            { typeof(Log), "logs" }
        }.ToFrozenDictionary();

        public static string GetCollectionName<T>()
        {
            if (CollectionNames.TryGetValue(typeof(T), out var name))
            {
                return name;
            }

            throw new NotSupportedException();
        }
    }
}
