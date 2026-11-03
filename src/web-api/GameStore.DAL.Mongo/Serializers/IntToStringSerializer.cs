using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.DAL.Mongo.Serializers
{
    public class IntToStringSerializer : SerializerBase<string?>
    {
        public override string? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.CurrentBsonType;

            switch (bsonType)
            {
                case BsonType.String:
                    return context.Reader.ReadString();
                case BsonType.Int32:
                    return context.Reader.ReadInt32().ToString();
                case BsonType.Int64:
                    return context.Reader.ReadInt64().ToString();
                case BsonType.Null:
                    context.Reader.ReadNull();
                    return null;
                default:
                    throw new BsonSerializationException($"Cannot convert BsonType {bsonType} to string.");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string? value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
            }
            else
            {
                context.Writer.WriteString(value);
            }
        }
    }
}