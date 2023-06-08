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
            _logger.LogInformation("Starting migration");

            await Task.Run(() =>
            {
                var connectionString = _configuration.GetConnectionString("AuditLog");
                if (connectionString == null)
                {
                    throw new ArgumentException("ConnectionString cannot be null");
                }
                var connectionStringPasswordHidden =
                    new SqlConnectionStringBuilder(connectionString)
                    {
                        Password = "********"
                    };
                _logger.LogInformation("Connection string is {ConnectionStringPasswordHidden}", connectionStringPasswordHidden.ConnectionString);

                EnsureDatabase.For.SqlDatabase(connectionString);

                var path = Path.GetDirectoryName(Process.GetCurrentProcess()?.MainModule?.FileName);
                _logger.LogInformation(path);
                
                //check if deployment scripts folder exists
                if (!Directory.Exists(Path.Combine(path, "DeploymentScripts")))
                {
                    _logger.LogError("DeploymentScripts folder not found");
                    Environment.ExitCode = 2;
                    _hostApplicationLifetime.StopApplication();
                    return;
                }
                
                //check if deployment scripts contains InitialCreate.sql
                if (!File.Exists(Path.Combine(path, "DeploymentScripts", "InitialCreate.sql")))
                {
                    _logger.LogError("InitialCreate.sql not found");
                    Environment.ExitCode = 2;
                    _hostApplicationLifetime.StopApplication();
                    return;
                }
                
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
