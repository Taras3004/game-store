using GameStore.Core.Interfaces.Repositories;
using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using GameStore.DAL.Data;
using GameStore.DAL.Repositories.Base;

namespace GameStore.DAL.Repositories
{
    internal class CommentRepository(GameStoreDbContext context) : SqlRepository<Comment>(context), ICommentRepository
    {
        private readonly GameStoreDbContext context = context;

        public async Task<int> BatchProductKeyUpdate(string oldKey, string newKey, CancellationToken token)
        {
            var query = context.Comments.Where(x => x.ProductKey == oldKey);

            return await query.ExecuteUpdateAsync(x => x.SetProperty(p => p.ProductKey, newKey), token);
        }
    }
}
