using FluentValidation.AspNetCore;
using GameStore.Core;
using GameStore.Web.Infrastructure;
using GameStore.Web.Infrastructure.Authorization.Permissions;
using GameStore.Web.Infrastructure.CustomMiddleware;
using GameStore.Web.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;
using GameStore.Core.Interfaces.Services;
using GameStore.Infrastructure.DataAccess;
using GameStore.Infrastructure.DomainEvents;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .WriteTo.File("logs/errors/errors-.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, restrictedToMinimumLevel: LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDomainEventDispatcher();

builder.Services.AddDataAccess(builder.Configuration);

string paymentApiUrl = builder.Configuration.GetValue<string>("PaymentApiUrl")
    ?? throw new InvalidOperationException();
builder.Services.AddCoreServices(paymentApiUrl);

builder.Services.AddCors(options =>
{
    options.AddPolicy("GameStoreCorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("x-total-numbers-of-games");
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration.GetValue<string>("JwtOptions:Secret")
                     ?? throw new InvalidOperationException();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Configuration
    .AddJsonFile("payment-methods.json", optional: false, reloadOnChange: true);

builder.Services.AddTransient<TotalGamesHeaderMiddleware>();
builder.Services.AddTransient<HttpLoggerMiddleware>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOutputCache();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddOptions<CacheOptions>()
    .BindConfiguration(CacheOptions.SectionName);
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserProvider, UserProvider>();

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpLoggerMiddleware();
app.UseExceptionHandler();
app.UseCors("GameStoreCorsPolicy");
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();
app.UseTotalGamesHeaderMiddleware();
app.MapControllers();

await app.RunAsync();