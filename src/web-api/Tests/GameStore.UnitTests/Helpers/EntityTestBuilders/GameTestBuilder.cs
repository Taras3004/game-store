using GameStore.Core.Dto.Details.Game;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Entities;
using GameStore.Models.ValueObjects;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class GameTestBuilder
    {
        public static Game CreateValidGame(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string? key = EntityBuilderConstants.ProvidedByBuilderString,
            string description = EntityBuilderConstants.ProvidedByBuilderString,
            GamePrice? gamePrice = null,
            int unitInStock = 0,
            Guid publisherId = default,
            string? legacyId = null,
            Guid id = default
            )
        {
            gamePrice ??= CreateValidGamePrice();
            return new Game(
                name,
                key,
                description,
                gamePrice,
                unitInStock,
                publisherId,
            id)
            {
                LegacyId = legacyId
            };
        }

        public static GameDetailsBase CreateValidGameDetailsBase(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string? key = EntityBuilderConstants.ProvidedByBuilderString,
            string? description = EntityBuilderConstants.ProvidedByBuilderString,
            int price = 0,
            int discount = 0,
            int unitInStock = 1)
        {
            return new GameDetailsBase
            {
                Name = name,
                Key = key,
                Description = description,
                Price = price,
                Discount = discount,
                UnitInStock = unitInStock,
            };
        }

        public static GameDetailsWithId CreateValidGameDetailsWithId(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string? key = EntityBuilderConstants.ProvidedByBuilderString,
            string? description = EntityBuilderConstants.ProvidedByBuilderString,
            int price = 0,
            int discount = 0,
            int unitInStock = 2,
            Guid id = default)
        {
            return new GameDetailsWithId
            {
                Name = name,
                Key = key,
                Description = description,
                Price = price,
                Discount = discount,
                UnitInStock = unitInStock,
                Id = id.ToString()
            };
        }

        public static GameResponse CreateValidGameResponse(
            string name = EntityBuilderConstants.ProvidedByBuilderString,
            string key = EntityBuilderConstants.ProvidedByBuilderString,
            string? description = EntityBuilderConstants.ProvidedByBuilderString,
            double price = 0,
            int discontinued = 0,
            int unitInStock = 2,
            Guid? publisherId = null,
            Guid id = default)
        {
            var pubId = publisherId ?? Guid.NewGuid();
            return new GameResponse
            {
                Id = id.ToString(),
                Name = name,
                Key = key,
                Description = description,
                Price = price,
                Discontinued = discontinued,
                UnitInStock = unitInStock,
                PublisherId = pubId.ToString(),
            };
        }

        public static GameFileResponse CreateValidGameFileResponse(
            byte[]? fileContent = null,
            string contentType = EntityBuilderConstants.ProvidedByBuilderString,
            string fileName = EntityBuilderConstants.ProvidedByBuilderString
        )
        {
            fileContent ??= new byte[10];
            return new GameFileResponse
            {
                FileContent = fileContent,
                ContentType = contentType,
                FileName = fileName
            };
        }

        private static GamePrice CreateValidGamePrice()
        {
            var result = GamePrice.Create(price: 10, discount: 5);

            if (result.IsFailure)
                throw new InvalidOperationException();

            return result.Value;
        }
    }
}