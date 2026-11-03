using GameStore.DAL.Data.PredefinedValues;
using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder
                .Property(x => x.Name)
                .HasMaxLength(GenreValidation.NameMaxLength);

            builder
                .HasOne(x => x.ParentGenre)
                .WithMany()
                .HasForeignKey(x => x.ParentGenreId);

            builder.HasData(PredefinedGenres.Genres);

            builder.Ignore(x => x.DomainEvents);
        }
    }
}
