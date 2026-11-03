using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class OrderDetails : NorthwindEntity
    {
        [BsonElement("OrderID")]
        public int OrderId { get; set; }
        [BsonElement("ProductID")]
        public int ProductId { get; set; }
        public int? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public int? Discount { get; set; }
    }
}
