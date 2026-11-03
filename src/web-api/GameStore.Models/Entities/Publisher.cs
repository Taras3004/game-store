using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class Publisher : BaseEntity, IAggregateRoot
    {
        public string CompanyName { get; private set; }
        public string? HomePage { get; private set; }
        public string? Description { get; private set; }
        public string? ContactName { get; init; }
        public string? ContactTitle { get; init; }
        public string? Address { get; init; }
        public string? City { get; init; }
        public string? Region { get; init; }
        public string? PostalCode { get; init; }
        public string? Country { get; init; }
        public string? Phone { get; init; }
        public string? Fax { get; init; }
        public string? LegacyId { get; init; }

        public IReadOnlyCollection<Game> Games { get; } = new List<Game>();

        public Publisher(string companyName, string? homePage, string? description, Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            CompanyName = companyName;
            HomePage = homePage;
            Description = description;
        }

        private Publisher()
        {
            CompanyName = null!;
        }

        public void UpdateDetails(string companyName, string? homePage, string? description)
        {
            CompanyName = companyName;
            HomePage = homePage;
            Description = description;
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
