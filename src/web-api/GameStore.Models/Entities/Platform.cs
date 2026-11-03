using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class Platform : BaseEntity, IAggregateRoot
    {
        public string Type { get; private set; }

        public IReadOnlyCollection<GamePlatform> GamePlatforms { get; } = new List<GamePlatform>();

        public Platform(string type, Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Type = type;
        }

        private Platform()
        {
            Type = null!;
        }

        public void UpdateDetails(string platformType)
        {
            Type = platformType;
        }

        public List<IDomainEvent> DomainEvents { get; } = [];
        public void Raise(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }
    }
}
