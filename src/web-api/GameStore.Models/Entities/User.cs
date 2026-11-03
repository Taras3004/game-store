using GameStore.Models.Entities.ConfigurationEntities;

namespace GameStore.Models.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string? Email { get; private set; }
        public IReadOnlyCollection<Role> Roles => roles;
        private List<Role> roles = [];

        public User(string username, string password, string? email = null, Guid? id = null)
        {
            Username = username;
            Password = password;
            Email = email;
            Id = id ?? Guid.NewGuid();
        }

        private User()
        {
            Username = null!;
            Password = null!;
            roles = null!;
        }

        public void SetRoles(List<Role> newRoles)
        {
            roles = newRoles;
        }

        public void Update(string username, string password, string? email = null)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
