using GameStore.Models.Constants.ValidationValues;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.Data.EntityTypeConfigurations
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .Property(x => x.Name)
                .HasMaxLength(CommentValidation.NameMaxLength);

            builder
                .Property(x => x.Body)
                .HasMaxLength(CommentValidation.BodyMaxLength);

            builder
                .HasOne(x => x.ParentComment)
                .WithMany(x => x.ChildComments)
                .HasForeignKey(x => x.ParentCommentId);

            builder
                .HasIndex(x => x.ProductKey)
                .IsClustered(false);

            builder.Ignore(x => x.DomainEvents);
            builder.HasQueryFilter(x => x.IsDeleted == false);

            builder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }
    }
}
