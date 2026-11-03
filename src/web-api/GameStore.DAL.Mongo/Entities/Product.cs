using GameStore.DAL.Mongo.Entities.ConfigurationEntities;
using GameStore.Models.Entities;
using GameStore.Models.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Mongo.Entities
{
    internal class Product : NorthwindEntity
    {
        [BsonElement("ProductID")]
        public int ProductId { get; set; }
        public required string ProductName { get; set; }

        [BsonElement("SupplierID")]
        public int SupplierId { get; set; }

        [BsonElement("CategoryID")]
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public int ReorderLevel { get; set; }
        public int Views { get; set; }
        public required string ProductKey { get; set; }

        public static Game? Map(Product product)
        {
            var gamePrice = GamePrice.Create(product.UnitPrice, 0);
            if (gamePrice.IsFailure)
            {
                return null;
            }

            return new Game(product.ProductName, product.ProductKey, null, gamePrice.Value, product.UnitsInStock, Guid.Empty);
        }
    }
}
