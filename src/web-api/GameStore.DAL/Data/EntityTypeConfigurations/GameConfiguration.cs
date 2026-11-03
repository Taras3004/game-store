using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(GameValidation.NameMaxLength);

            builder
                .HasIndex(x => x.Key)
                .IsUnique();

            builder
                .Property(x => x.Key)
                .HasMaxLength(GameValidation.KeyMaxLength);

            builder
                .Property(x => x.Description)
                .HasMaxLength(GameValidation.DescriptionMaxLength);

            builder
                .ComplexProperty(x => x.GamePrice, propertyBuilder =>
                {
                    propertyBuilder.Property(x => x.Price)
                        .HasColumnName("Price")
                        .IsRequired();

                    propertyBuilder.Property(x => x.Discount)
                        .HasColumnName("Discount")
                        .IsRequired();
                });

            builder
                .HasOne(x => x.Publisher)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.PublisherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(x => x.UnitInStock)
                .IsConcurrencyToken();

            builder.Ignore(x => x.DomainEvents);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
