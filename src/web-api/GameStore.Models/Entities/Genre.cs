using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class Genre : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Guid? ParentGenreId { get; private set; }
        public Genre? ParentGenre { get; private set; }
        public string? Description { get; init; }
        public string? Picture { get; init; }
        public string? LegacyId { get; init; }

        public IReadOnlyCollection<GameGenre> GameGenres { get; } = new List<GameGenre>();

        public Genre(string name, Guid? parentGenreId = null, Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name;
            ParentGenreId = parentGenreId;
        }

        public void SetParent(Genre parent)
        {
            ArgumentNullException.ThrowIfNull(parent);

            ParentGenre = parent;
            ParentGenreId = parent.Id;
        }

        private Genre()
        {
            Name = null!;
        }

        public void UpdateDetails(string genreName, Guid? parentGenreId)
        {
            Name = genreName;
            ParentGenreId = parentGenreId;
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
