using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Repositories;
using GameStore.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.DAL.Data
{
    internal class GameStoreUnitOfWork(
        GameStoreDbContext context,
        IServiceProvider serviceProvider) : IUnitOfWork
    {
        public IGameRepository Games => serviceProvider.GetService<IGameRepository>()
                                            ?? throw new InvalidOperationException();

        public IGenreRepository Genres => serviceProvider.GetService<IGenreRepository>()
                                            ?? throw new InvalidOperationException();

        public IPlatformRepository Platforms => serviceProvider.GetService<IPlatformRepository>()
                                            ?? throw new InvalidOperationException();

        public IPublisherRepository Publishers => serviceProvider.GetService<IPublisherRepository>()
                                            ?? throw new InvalidOperationException();

        public IOrderRepository Orders => serviceProvider.GetService<IOrderRepository>()
                                    ?? throw new InvalidOperationException();

        public ICommentRepository Comments => serviceProvider.GetService<ICommentRepository>()
                            ?? throw new InvalidOperationException();
        public IOrderProductRepository OrderProducts => serviceProvider.GetService<IOrderProductRepository>()
                            ?? throw new InvalidOperationException();

        public IBanRecordRepository BanRecords => serviceProvider.GetService<IBanRecordRepository>()
                            ?? throw new InvalidOperationException();

        public IRoleRepository Roles => serviceProvider.GetService<IRoleRepository>()
                                          ?? throw new InvalidOperationException();

        public IUserRepository Users => serviceProvider.GetService<IUserRepository>()
                                          ?? throw new InvalidOperationException();

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConcurrencyException();
            }
            catch (DbUpdateException)
            {
                throw new UpdateException();
            }
        }
    }
}
