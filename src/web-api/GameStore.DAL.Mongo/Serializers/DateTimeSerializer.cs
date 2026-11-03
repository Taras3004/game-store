using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.DAL.Mongo.Serializers
{
    public class DateTimeSerializer : SerializerBase<DateTime?>
    {
        public override DateTime? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return null;
            }

            if (bsonType == BsonType.String)
            {
                var dateString = context.Reader.ReadString();

                if (string.IsNullOrWhiteSpace(dateString))
                {
                    return null;
                }

                if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, out var parsedDate))
                {
                    return parsedDate;
                }

                return null;
            }

            if (bsonType == BsonType.DateTime)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(context.Reader.ReadDateTime()).UtcDateTime;
            }

            context.Reader.SkipValue();
            return null;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime? value)
        {
            if (value.HasValue)
            {
                context.Writer.WriteString(value.Value.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
            }
            else
            {
                context.Writer.WriteNull();
            }
        }
    }
}