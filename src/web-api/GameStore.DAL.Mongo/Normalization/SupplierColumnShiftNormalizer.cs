using GameStore.DAL.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// Suppliers have two shapes of shift:
    /// a comma in CompanyName ("Pavlova, Ltd.") leaves the name suffix in ContactName,
    /// a comma in Address ("Viale Dante, 75") leaves the second half of the address in City.
    /// </summary>
    internal class SupplierColumnShiftNormalizer(INorthwindQuerySource<Supplier> inner)
        : ColumnShiftNormalizer<Supplier>(inner)
    {
        private static readonly BsonArray CompanyNameSuffixes = ["Ltd.", "Mate"];

        protected override string OverflowField => "field12";

        protected override IEnumerable<(string Name, BsonValue ShiftedValue)> GetShiftedFields()
        {
            var isNameShift = new BsonDocument("$in", new BsonArray { Field(nameof(Supplier.ContactName)), CompanyNameSuffixes });

            BsonValue ByShape(BsonValue nameShifted, BsonValue addressShifted) =>
                new BsonDocument("$cond", new BsonArray { isNameShift, nameShifted, addressShifted });

            yield return (nameof(Supplier.CompanyName), ByShape(
                Join(nameof(Supplier.CompanyName), nameof(Supplier.ContactName)),
                Field(nameof(Supplier.CompanyName))));
            yield return (nameof(Supplier.ContactName), ByShape(
                Field(nameof(Supplier.ContactTitle)),
                Field(nameof(Supplier.ContactName))));
            yield return (nameof(Supplier.ContactTitle), ByShape(
                AsString(nameof(Supplier.Address)),
                Field(nameof(Supplier.ContactTitle))));
            yield return (nameof(Supplier.Address), ByShape(
                AsString(nameof(Supplier.City)),
                Join(nameof(Supplier.Address), nameof(Supplier.City))));

            yield return (nameof(Supplier.City), AsString(nameof(Supplier.Region)));
            yield return (nameof(Supplier.Region), AsString(nameof(Supplier.PostalCode)));
            yield return (nameof(Supplier.PostalCode), AsString(nameof(Supplier.Country)));
            yield return (nameof(Supplier.Country), AsString(nameof(Supplier.Phone)));
            yield return (nameof(Supplier.Phone), AsString(nameof(Supplier.Fax)));
            yield return (nameof(Supplier.Fax), AsString(nameof(Supplier.HomePage)));
            yield return (nameof(Supplier.HomePage), AsString(OverflowField));
        }
    }
}
