using GameStore.Core.Interfaces.Repositories;
using GameStore.Core.Specifications.BaseClass;
using GameStore.Models.Entities.ConfigurationEntities;
using Moq;

namespace GameStore.UnitTests.Core.Helpers
{
    public static class RepositoryMockExtensions
    {
        public static void SetupGetFirstOrDefaultVerifiable<TRepo, TEntity, TSpec>(
            this Mock<TRepo> mock,
            TEntity? returnValue,
            Func<TSpec, bool>? match = null)
            where TRepo : class, IRepository<TEntity>
            where TEntity : BaseEntity
            where TSpec : Specification<TEntity>
        {
            mock
                .Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.Is<Specification<TEntity>>(s => s is TSpec 
                                                       && (match == null || match((TSpec)s))),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue)
                .Verifiable();
        }

        public static void SetupListVerifiable<TRepo, TEntity, TSpec>(
            this Mock<TRepo> mock,
            IEnumerable<TEntity> returnValue,
            Func<TSpec, bool>? match = null)
            where TRepo : class, IRepository<TEntity>
            where TEntity : BaseEntity
            where TSpec : Specification<TEntity>
        {
            mock
                .Setup(repo => repo.GetListAsync(
                    It.Is<Specification<TEntity>>(s => s is TSpec 
                                                       && (match == null || match((TSpec)s))),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue)
                .Verifiable();
        }
    }
}
