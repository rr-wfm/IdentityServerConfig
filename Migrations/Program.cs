using IdentityServerConfig.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
{
    services.AddHostedService<Worker>();
}).UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithMachineName()
        .Enrich.WithProcessId()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("LogicalServiceName", "IdentityServerConfig.Migrations");
}).Build();

await host.RunAsync();