using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class GamePlatformConfiguration : IEntityTypeConfiguration<GamePlatform>
    {
        public void Configure(EntityTypeBuilder<GamePlatform> builder)
        {
            builder
                .HasKey(x => new { x.GameId, x.PlatformId });

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.GamePlatforms)
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.Platform)
                .WithMany(x => x.GamePlatforms)
                .HasForeignKey(x => x.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.Game.IsDeleted);
        }
    }
}
