using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class BanRecord : BaseEntity, IAggregateRoot
    {
        public string Username { get; private set; }
        public string BanDuration { get; private set; }
        public DateTime BannedUntil { get; private set; }

        public BanRecord(string username, string banDuration, DateTime bannedUntil)
        {
            Username = username;
            BanDuration = banDuration;
            BannedUntil = bannedUntil;
        }

        private BanRecord()
        {
            Username = null!;
            BanDuration = null!;
        }

        public void UpdateRecord(DateTime bannedUntil)
        {
            BannedUntil = bannedUntil;
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