using GameStore.DAL.Data.PredefinedValues;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class PlatformConfiguration : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder
                .Property(x => x.Type)
                .HasMaxLength(PlatformValidation.TypeMaxLength);

            builder.HasData(PredefinedPlatforms.Platforms);

            builder.Ignore(x => x.DomainEvents);
        }
    }
}
