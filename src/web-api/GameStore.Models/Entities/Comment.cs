using GameStore.Models.DomainEvents;
using GameStore.Models.Entities.ConfigurationEntities;
using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class Comment : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Body { get; private set; }
        public string ProductKey { get; private set; }
        public bool IsDeleted { get; private set; }
        public CommentType CommentType { get; private set; }
        public Guid? ParentCommentId { get; private set; }
        public Comment? ParentComment { get; private set; }
        public IReadOnlyCollection<Comment> ChildComments { get; private set; } = [];
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Comment(string name, string body, CommentType commentType, Comment parentComment)
        {
            Name = name;
            Body = body;
            ProductKey = parentComment.ProductKey;
            CommentType = commentType;
            ParentComment = parentComment;
            ParentCommentId = parentComment.Id;
            UserId = parentComment.UserId;
            User = parentComment.User;
        }

        public Comment(string name, string body, string productKey, Guid userId)
        {
            Name = name;
            ProductKey = productKey;
            Body = body;
            CommentType = CommentType.Default;
            UserId = userId;
            User = null!;
        }


        private Comment()
        {
            Name = null!;
            Body = null!;
            ProductKey = null!;
            User = null!;
        }

        public void Update(string productKey)
        {
            ProductKey = productKey;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public List<IDomainEvent> DomainEvents { get; } = [];
        public void Raise(IDomainEvent domainEvent)
        {
            DomainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }
    }
}