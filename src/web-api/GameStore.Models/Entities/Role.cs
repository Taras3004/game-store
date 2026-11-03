using GameStore.Models.Entities.ConfigurationEntities;
using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public List<Permission> Permissions { get; private set; }

        public Role(string name, List<Permission>? permissions = null, Guid id = default)
        {
            Name = name;
            Permissions = permissions ?? [];
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }

        private Role()
        {
            Name = null!;
            Permissions = null!;
        }

        public void SetPermissions(List<Permission> newPermissions)
        {
            Permissions = newPermissions;
        }

        public void Update(string name)
        {
            Name = name;
        }
    }
}
