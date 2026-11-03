using GameStore.DAL.Data.PredefinedValues;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(UserValidation.UsernameMaxLength);

            builder
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(UserValidation.PasswordMaxLength);

            builder.HasData(PredefinedUsers.Users);

            builder
                .HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "RoleUser",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RolesId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    je =>
                    {
                        je.HasData(
                            new { UserId = PredefinedUsers.UserId, RolesId = PredefinedRoles.Data.UserId },
                            new { UserId = PredefinedUsers.ModeratorId, RolesId = PredefinedRoles.Data.ModeratorId },
                            new { UserId = PredefinedUsers.ManagerId, RolesId = PredefinedRoles.Data.ManagerId },
                            new { UserId = PredefinedUsers.AdminId, RolesId = PredefinedRoles.Data.AdminId }
                        );
                    });
        }
    }
}