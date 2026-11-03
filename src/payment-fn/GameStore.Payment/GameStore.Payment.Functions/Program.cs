using Azure.Monitor.OpenTelemetry.Exporter;
using GameStore.Payment.Functions.Options;
using GameStore.Payment.Functions.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddOptions<PaymentOptions>().BindConfiguration(PaymentOptions.SectionName);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<AccountStore>();
builder.Services.AddSingleton<PaymentProcessor>();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

await builder.Build().RunAsync();
