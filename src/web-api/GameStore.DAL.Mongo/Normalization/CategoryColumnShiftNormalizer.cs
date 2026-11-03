using GameStore.DAL.Mongo.Entities;
using MongoDB.Bson;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// A quote closed too early in Description ("..., spreads", and seasonings) leaves
    /// the rest of the description in Picture and the picture in the overflow field.
    /// </summary>
    internal class CategoryColumnShiftNormalizer(INorthwindQuerySource<Category> inner)
        : ColumnShiftNormalizer<Category>(inner)
    {
        protected override string OverflowField => "field4";

        protected override IEnumerable<(string Name, BsonValue ShiftedValue)> GetShiftedFields()
        {
            yield return (nameof(Category.Description), Join(nameof(Category.Description), nameof(Category.Picture)));
            yield return (nameof(Category.Picture), AsString(OverflowField));
        }
    }
}
