using FluentValidation;
using GameStore.Core.Interfaces;
using GameStore.Core.Interfaces.Payment;
using GameStore.Core.Interfaces.Services;
using GameStore.Core.Payment;
using GameStore.Core.Payment.Options;
using GameStore.Core.Payment.Options.PaymentGateway;
using GameStore.Core.Payment.PaymentMethods;
using GameStore.Core.Services;
using GameStore.Core.Services.AccessCheckers;
using GameStore.Core.Users.Options;
using GameStore.Models.Constants;
using GameStore.Models.DomainEvents;
using GameStore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace GameStore.Core
{
    public static class ServiceCollectionExtension
    {
        public static void AddCoreServices(this IServiceCollection services, string paymentApiUrl)
        {
            services.AddPublicServices();
            services.AddScoped<IPlatformInternalService, PlatformService>();

            services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly);

            services.AddPaymentDependencies(paymentApiUrl);

            services.AddDomainEventHandlers();

            services.AddAuth();

            services.AddAccessCheckers();
        }

        private static void AddPublicServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IPlatformService, PlatformService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IBanService, BanService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginService, LoginService>();
        }

        private static void AddPaymentDependencies(this IServiceCollection services, string paymentApiUrl)
        {
            services.AddHttpClient(HttpClients.PaymentHttpClientName, client =>
            {
                client.BaseAddress = ToBaseAddress(paymentApiUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());

            services.AddOptions<PaymentGatewayOptions>()
                .BindConfiguration(PaymentGatewayOptions.SectionName);
            services.AddOptions<PaymentOptions>()
                .BindConfiguration(PaymentOptions.SectionName);

            services.AddKeyedScoped<IPaymentMethod, BankPaymentMethod>(PaymentMethods.BankMethodName);
            services.AddKeyedScoped<IPaymentMethod, IboxPaymentMethod>(PaymentMethods.IBoxMethodName);
            services.AddKeyedScoped<IPaymentMethod, VisaPaymentMethod>(PaymentMethods.VisaMethodName);

            services.AddScoped<IPaymentMethodFactory, PaymentMethodFactory>();
        }

        /// <summary>
        /// Ensures a trailing slash so relative request paths are appended to the base path
        /// (e.g. "https://host/api" + "users" => "https://host/api/users", not "https://host/users").
        /// </summary>
        private static Uri ToBaseAddress(string url) =>
            new(url.EndsWith('/') ? url : url + "/");

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 1,
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(2));
        }

        private static void AddDomainEventHandlers(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssembliesOf(typeof(ServiceCollectionExtension))
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        private static void AddAuth(this IServiceCollection services)
        {
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName);

            services.AddOptions<ExternalAuthServiceOptions>()
                .BindConfiguration(ExternalAuthServiceOptions.SectionName);

            services.AddHttpClient<ExternalAuthService>((serviceProvider, httpClient) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<ExternalAuthServiceOptions>>();
                httpClient.BaseAddress = ToBaseAddress(options.Value.BaseAddress);
            });

            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenProvider, TokenProvider>();
        }

        private static void AddAccessCheckers(this IServiceCollection services)
        {
            services.AddScoped<IEntityAccessCheckerFactory, EntityAccessCheckerFactory>();

            services.AddKeyedScoped<IEntityAccessChecker, CommentAccessChecker>(nameof(Comment));
            services.AddKeyedScoped<IEntityAccessChecker, GameAccessChecker>(nameof(Game));
            services.AddKeyedScoped<IEntityAccessChecker, GenreAccessChecker>(nameof(Genre));
            services.AddKeyedScoped<IEntityAccessChecker, OrderAccessChecker>(nameof(Order));
            services.AddKeyedScoped<IEntityAccessChecker, PlatformAccessChecker>(nameof(Platform));
            services.AddKeyedScoped<IEntityAccessChecker, PublisherAccessChecker>(nameof(Publisher));
            services.AddKeyedScoped<IEntityAccessChecker, RoleAccessChecker>(nameof(Role));
            services.AddKeyedScoped<IEntityAccessChecker, UserAccessChecker>(nameof(User));
        }
    }
}