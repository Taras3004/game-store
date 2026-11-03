using GameStore.Models.Entities;

namespace GameStore.Core.Interfaces.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<int> BatchProductKeyUpdate(string oldKey, string newKey, CancellationToken token);
    }
}
