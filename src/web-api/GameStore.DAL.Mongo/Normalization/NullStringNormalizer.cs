using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DAL.Mongo.Normalization
{
    /// <summary>
    /// The Northwind export wrote SQL NULLs as the literal text "NULL".
    /// Replaces it with a real null in every string field of the document.
    /// </summary>
    /// <remarks>Must be registered after the column shift normalizers, so shifted "NULL" values are already in place.</remarks>
    internal class NullStringNormalizer<TDocument>(INorthwindQuerySource<TDocument> inner) : INorthwindQuerySource<TDocument>
        where TDocument : NorthwindEntity
    {
        private const string NullLiteral = "NULL";

        private static readonly BsonDocument Stage = BuildStage();

        public IQueryable<TDocument> Query() =>
            inner.Query().AppendStage<TDocument, TDocument>(Stage);

        private static BsonDocument BuildStage()
        {
            var classMap = BsonClassMap.LookupClassMap(typeof(TDocument));

            var fields = new BsonDocument();
            foreach (var memberMap in classMap.AllMemberMaps)
            {
                if (memberMap == classMap.IdMemberMap || memberMap.MemberType != typeof(string))
                {
                    continue;
                }

                var field = new BsonString($"${memberMap.ElementName}");
                fields[memberMap.ElementName] = new BsonDocument("$cond", new BsonArray
                {
                    new BsonDocument("$eq", new BsonArray { field, NullLiteral }),
                    BsonNull.Value,
                    field
                });
            }

            return new BsonDocument("$set", fields);
        }
    }
}
