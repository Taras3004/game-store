using Moq;

namespace GameStore.UnitTests.Core.Helpers
{
    internal static class ParameterProvider
    {
        public static readonly CancellationToken AnyToken = It.IsAny<CancellationToken>();
    }
}
