using GameStore.Core.Dto.Details.Platform;
using GameStore.Core.Dto.Responses;
using GameStore.Models.Entities;

namespace GameStore.UnitTests.Helpers.EntityTestBuilders
{
    internal static class PlatformTestBuilder
    {
        public static Platform CreateValidPlatform(
            string type = EntityBuilderConstants.ProvidedByBuilderString,
            Guid id = default)
        {
            return new Platform(
                type,
                id);
        }

        public static PlatformDetailsBase CreateValidPlatformDetailsBase(
            string type = EntityBuilderConstants.ProvidedByBuilderString)
        {
            return new PlatformDetailsBase
            {
                Type = type
            };
        }

        public static PlatformDetailsWithId CreateValidPlatformDetailsWithId(
            string type = EntityBuilderConstants.ProvidedByBuilderString,
            Guid id = default)
        {
            return new PlatformDetailsWithId
            {
                Type = type,
                Id = id == Guid.Empty ? Guid.NewGuid() : id
            };
        }

        public static PlatformResponse CreateValidPlatformResponse(
            string type = EntityBuilderConstants.ProvidedByBuilderString,
            Guid id = default)
        {
            return new PlatformResponse
            {
                Type = type,
                Id = id == Guid.Empty ? Guid.NewGuid() : id
            };
        }
    }
}