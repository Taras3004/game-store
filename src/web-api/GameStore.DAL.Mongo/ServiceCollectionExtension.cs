using GameStore.Core.Filtering.Contracts;
using GameStore.Core.Interfaces.Repositories;
using GameStore.DAL.Mongo.Entities;
using GameStore.DAL.Mongo.Normalization;
using GameStore.DAL.Mongo.Options;
using GameStore.DAL.Mongo.Repositories;
using GameStore.DAL.Mongo.Repositories.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameStore.DAL.Mongo
{
    public static class ServiceCollectionExtension
    {
        public static void AddMongoDbDependencies(this IServiceCollection services, string dataSourceKey)
        {
            services.AddOptions<MongoDbOptions>()
                .BindConfiguration(MongoDbOptions.SectionName);

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });

            services.AddScoped(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();

                var options = serviceProvider.GetRequiredService<IOptionsSnapshot<MongoDbOptions>>().Value;

                return client.GetDatabase(options.DatabaseName);
            });

            services.AddScoped(typeof(MongoKeyResolver<>));

            AddNorthwindQuerySources(services);

            AddMongoRepositories(services, dataSourceKey);

            AddEntityFiltersAndSteps(services);
        }

        private static void AddNorthwindQuerySources(IServiceCollection services)
        {
            // Closed registrations: Scrutor can't decorate an open generic registration
            services.AddScoped<INorthwindQuerySource<Product>, NorthwindQuerySource<Product>>();
            services.AddScoped<INorthwindQuerySource<Supplier>, NorthwindQuerySource<Supplier>>();
            services.AddScoped<INorthwindQuerySource<Category>, NorthwindQuerySource<Category>>();
            services.AddScoped<INorthwindQuerySource<Order>, NorthwindQuerySource<Order>>();

            // Stages run in registration order: column shifts first, then "NULL" literals in their final fields
            services.Decorate<INorthwindQuerySource<Supplier>, SupplierColumnShiftNormalizer>();
            services.Decorate<INorthwindQuerySource<Order>, OrderColumnShiftNormalizer>();
            services.Decorate<INorthwindQuerySource<Category>, CategoryColumnShiftNormalizer>();
            services.Decorate(typeof(INorthwindQuerySource<>), typeof(NullStringNormalizer<>));
        }

        private static void AddEntityFiltersAndSteps(IServiceCollection services)
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

        private static void AddMongoRepositories(IServiceCollection services, string dataSourceKey)
        {
            services.AddKeyedScoped<IGameRepository, MongoGameRepository>(dataSourceKey);
            services.AddKeyedScoped<IGenreRepository, MongoGenreRepository>(dataSourceKey);
            services.AddKeyedScoped<IPublisherRepository, MongoPublisherRepository>(dataSourceKey);
            services.AddKeyedScoped<IOrderRepository, MongoOrderRepository>(dataSourceKey);
            services.AddScoped<ILogRepository, LogRepository>();
        }
    }
}
