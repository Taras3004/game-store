using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class BanRecordConfiguration : IEntityTypeConfiguration<BanRecord>
    {
        public void Configure(EntityTypeBuilder<BanRecord> builder)
        {
            builder.Property(x => x.Username)
                .HasMaxLength(CommentValidation.NameMaxLength);

            builder.HasIndex(x => x.Username)
                .IsUnique();

            builder.Property(x => x.BannedUntil)
                .IsConcurrencyToken();

            builder.Ignore(x => x.DomainEvents);
        }
    }
}