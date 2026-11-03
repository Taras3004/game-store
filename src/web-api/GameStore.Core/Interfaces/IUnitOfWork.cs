using GameStore.Core.Interfaces.Repositories;

namespace GameStore.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGameRepository Games { get; }
        IGenreRepository Genres { get; }
        IPlatformRepository Platforms { get; }
        IPublisherRepository Publishers { get; }
        IOrderRepository Orders { get; }
        IOrderProductRepository OrderProducts { get; }
        ICommentRepository Comments { get; }
        IBanRecordRepository BanRecords { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
