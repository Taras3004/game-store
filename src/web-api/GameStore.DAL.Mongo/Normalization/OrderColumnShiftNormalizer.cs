using GameStore.DAL.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// A comma in ShipAddress ("Rua do Paço, 67") leaves the house number in ShipCity
    /// and the real country in the overflow field.
    /// </summary>
    internal class OrderColumnShiftNormalizer(INorthwindQuerySource<Order> inner)
        : ColumnShiftNormalizer<Order>(inner)
    {
        protected override string OverflowField => "field14";

        protected override IEnumerable<(string Name, BsonValue ShiftedValue)> GetShiftedFields()
        {
            yield return (nameof(Order.ShipAddress), Join(nameof(Order.ShipAddress), nameof(Order.ShipCity)));
            yield return (nameof(Order.ShipCity), AsString(nameof(Order.ShipRegion)));
            yield return (nameof(Order.ShipRegion), AsString(nameof(Order.ShipPostalCode)));
            yield return (nameof(Order.ShipPostalCode), AsString(nameof(Order.ShipCountry)));
            yield return (nameof(Order.ShipCountry), AsString(OverflowField));
        }
    }
}
