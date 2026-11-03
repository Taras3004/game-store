namespace GameStore.Models.Entities.ConfigurationEntities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; }

        public override int GetHashCode() => Id.GetHashCode();
        public override bool Equals(object? obj)
        {
            return obj is BaseEntity other && other.Id == Id;
        }
    }
}
