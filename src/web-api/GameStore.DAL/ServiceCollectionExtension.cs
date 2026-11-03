using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Repositories;
using GameStore.DAL.Data;
using GameStore.DAL.Data.Interceptors;
using GameStore.DAL.Repositories;
using GameStore.DAL.Sql.Repositories;
using GameStore.DAL.TransactionalOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.DAL
{
    public static class ServiceCollectionExtension
    {
        public static void AddSqlDependencies(this IServiceCollection services, string connectionString, string dataSourceKey)
        {
            services.AddScoped<OutboxMessageSaveChangesInterceptor>();

            services.AddDbContext<GameStoreDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString);

                var outboxMessageInterceptor = serviceProvider.GetRequiredService<OutboxMessageSaveChangesInterceptor>();
                options.AddInterceptors(outboxMessageInterceptor);
            });

            services.AddScoped<IUnitOfWork, GameStoreUnitOfWork>();
            services.AddKeyedScoped<IGameRepository, GameRepository>(dataSourceKey);
            services.AddKeyedScoped<IGenreRepository, GenreRepository>(dataSourceKey);
            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddKeyedScoped<IPublisherRepository, PublisherRepository>(dataSourceKey);
            services.AddKeyedScoped<IOrderRepository, OrderRepository>(dataSourceKey);
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IBanRecordRepository, BanRecordRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderProductRepository, OrderProductRepository>();

            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

            services.AddEntityFiltersAndSteps();
        }

        private static void AddEntityFiltersAndSteps(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssembliesOf(typeof(ServiceCollectionExtension))
                .AddClasses(classes => classes.AssignableTo(typeof(IFilterStep<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan.FromAssembliesOf(typeof(ServiceCollectionExtension))
                .AddClasses(classes => classes.AssignableTo(typeof(IEntityFilter<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
    }
}
