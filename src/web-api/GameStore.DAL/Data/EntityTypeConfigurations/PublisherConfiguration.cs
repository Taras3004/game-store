using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder
                .HasIndex(x => x.CompanyName)
                .IsUnique();

            builder
                .Property(x => x.CompanyName)
                .HasMaxLength(PublisherValidation.CompanyNameMaxLength);

            builder
                .Property(x => x.HomePage)
                .HasMaxLength(PublisherValidation.HomepageMaxLength);

            builder
                .Property(x => x.Description)
                .HasMaxLength(PublisherValidation.DescriptionMaxLength);

            builder.Ignore(x => x.DomainEvents);
        }
    }
}
