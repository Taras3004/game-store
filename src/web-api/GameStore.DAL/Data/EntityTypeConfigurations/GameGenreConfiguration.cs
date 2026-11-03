using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder
                .HasKey(x => new { x.GameId, x.GenreId });

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.GameGenres)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Genre)
                .WithMany(x => x.GameGenres)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.Game.IsDeleted);
        }
    }
}
