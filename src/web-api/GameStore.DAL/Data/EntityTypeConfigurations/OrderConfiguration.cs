using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(x => x.Status)
                .HasConversion<string>();

            builder.Ignore(x => x.DomainEvents);

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
