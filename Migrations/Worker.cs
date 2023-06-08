using System.Diagnostics;
using DbUp.Reboot;
using DbUp.Reboot.Engine;
using DbUp.Reboot.ScriptProviders;
using DbUp.Reboot.Support;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServerConfig.Migrations;

public class Worker : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="Worker" /> class.
    /// </summary>
    public Worker(ILogger<Worker> logger, IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogError("Starting migration");

            await Task.Run(() =>
            {
                var connectionString = _configuration.GetConnectionString("AuditLog");
                if (connectionString == null)
                {
                    throw new ArgumentException("ConnectionString cannot be null");
                }
                _logger.LogError("Connection string is {ConnectionString}", connectionString);

                // try
                // {
                //     var sqlConnection = new SqlConnection(connectionString);
                //     sqlConnection.Open();
                // }
                // catch (Exception e)
                // {
                //     throw new ArgumentException("Could not connect to database", e);
                // }



                EnsureDatabase.For.SqlDatabase(connectionString);

                var path = Path.GetDirectoryName(Process.GetCurrentProcess()?.MainModule?.FileName);
                _logger.LogInformation(path);
                var upgradeEngineBuilder = DeployChanges.To
                    .SqlDatabase(connectionString, null)
                    .WithScriptsFromFileSystem(path,
                        new FileSystemScriptOptions { IncludeSubDirectories = true, Filter = (x) => x.Contains("DeploymentScripts") },
                        new SqlScriptOptions { ScriptType = ScriptType.RunOnce, RunGroupOrder = 1 })
                    .WithTransaction()
                    .LogToConsole();

                var upgrader = upgradeEngineBuilder.Build();
                _logger.LogInformation("Is upgrade required: " + upgrader.IsUpgradeRequired());

                var result = upgrader.PerformUpgrade();

                // Display the result
                if (result.Successful)
                {
                    _logger.LogInformation("Success!");
                }
                else
                {
                    _logger.LogError(result.Error, "Migration failed.");
                    Environment.ExitCode = 2;
                }

                _logger.LogInformation("Finished executing migrations, shutting down");
                _hostApplicationLifetime.StopApplication();
            }, stoppingToken);
        }
        catch
        {
            Environment.ExitCode = 1;
            throw;
        }
    }
}
