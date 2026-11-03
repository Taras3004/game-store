using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class OrderGameConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder
                .HasIndex(x => new { x.OrderId, x.ProductKey });

            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderProducts)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(x => x.ProductKey)
                .IsClustered(false);
        }
    }
}
