using GameStore.DAL.Data.PredefinedValues;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(RoleValidation.NameMaxLength);

            builder
                .PrimitiveCollection(e => e.Permissions)
                .ElementType()
                .HasConversion<string>();

            builder.HasData(PredefinedRoles.Roles);
        }
    }
}
