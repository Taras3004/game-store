using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// Repairs documents where an unquoted comma in the source CSV pushed every following column
    /// one position to the right. Such documents carry an extra overflow field (e.g. "field12").
    /// </summary>
    /// <remarks>
    /// Uses $set instead of $replaceWith: all expressions of one $set read the original document,
    /// so the columns can be moved in a single stage, and MongoDB can still move a later $match
    /// on untouched fields (ids, keys) in front of it and use indexes.
    /// </remarks>
    internal abstract class ColumnShiftNormalizer<TDocument>(INorthwindQuerySource<TDocument> inner)
        : INorthwindQuerySource<TDocument>
        where TDocument : NorthwindEntity
    {
        protected abstract string OverflowField { get; }

        public IQueryable<TDocument> Query()
        {
            var isShifted = new BsonDocument("$ne", new BsonArray
            {
                new BsonDocument("$type", Field(OverflowField)),
                "missing"
            });

            var fields = new BsonDocument();
            foreach (var (name, shiftedValue) in GetShiftedFields())
            {
                fields[name] = new BsonDocument("$cond", new BsonArray { isShifted, shiftedValue, Field(name) });
            }

            return inner.Query()
                .AppendStage<TDocument, TDocument>(new BsonDocument("$set", fields))
                .AppendStage<TDocument, TDocument>(new BsonDocument("$unset", OverflowField));
        }

        /// <summary>The repaired value of every affected field, applied only to shifted documents.</summary>
        protected abstract IEnumerable<(string Name, BsonValue ShiftedValue)> GetShiftedFields();

        protected static BsonString Field(string name) => new($"${name}");

        protected static BsonDocument AsString(string name) => new("$toString", Field(name));

        protected static BsonDocument Join(string first, string second) => new("$concat", new BsonArray
        {
            AsString(first),
            ", ",
            AsString(second)
        });
    }
}
