using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
