using GameStore.Core.Interfaces.Repositories;
using GameStore.DAL;
using GameStore.DAL.BackgroundServices;
using GameStore.DAL.Mongo;
using GameStore.Infrastructure.DataAccess.CompositeRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Infrastructure.DataAccess
{
    public static class ServiceCollectionExtension
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            string mssqlConnectionString = configuration.GetConnectionString("EFConnectionString") ??
                throw new ArgumentException("EFConnectionString is not provided in configuration");
            services.AddHostedService<OutboxProcessorBackgroundService>();

            services.AddSqlDependencies(mssqlConnectionString, DataSourceKeys.PrimaryDataSource);

            services.AddMongoDbDependencies(DataSourceKeys.LegacyDataSource);

            services.AddScoped<IGameRepository, CompositeGameRepository>();
            services.AddScoped<IGenreRepository, CompositeGenreRepository>();
            services.AddScoped<IPublisherRepository, CompositePublisherRepository>();
            services.AddScoped<IOrderRepository, CompositeOrderRepository>();
        }
    }
}
