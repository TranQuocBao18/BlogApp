using DbUp;
using DbUp.Engine;
using DbUp.Support;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Migration;

class Program
{
    private static IConfiguration? _configuration;

    private enum DBName
    {
        identity = 0,
        application = 1,
        communication = 2
    }

    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var lastArg = 0;

        for (; lastArg < args.Length; lastArg++)
        {
            if (IsArg(args[lastArg], "identity"))
            {
                Log.Information("Run migration - Identity Db");
                Run(DBName.identity, "identity");
                continue;
            }
            if (IsArg(args[lastArg], "application"))
            {
                Log.Information("Run migration - Db");
                Run(DBName.application, "application");
                continue;
            }
            if (IsArg(args[lastArg], "communication"))
            {
                Log.Information("Run migration - Db");
                Run(DBName.communication, "communication");
                continue;
            }

            else
            {
                throw new ArgumentOutOfRangeException($"{args[lastArg]} not found.");
            }
        }
    }

    private static bool IsArg(string candidate, string name)
    {
        return (name != null && candidate.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static void Run(DBName dBName, string schemaName)
    {
        var connString = _configuration.GetConnectionString(dBName.ToString());
        Log.Information($"Database information. dBName={dBName.ToString()} connString={connString}");
        var scriptFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Scripts/{dBName}");
        EnsureDatabase.For.SqlDatabase(connString);

        Log.Information($"Run schemaName={schemaName}");

        var upgradeBuilder = DeployChanges.To
            .SqlDatabase(connString, schemaName)
            .WithScriptsFromFileSystem(Path.Combine(scriptFolderPath, "Scripts"), new SqlScriptOptions { RunGroupOrder = 1, ScriptType = ScriptType.RunOnce })
            .WithScriptsFromFileSystem(Path.Combine(scriptFolderPath, "SeedData"), new SqlScriptOptions { RunGroupOrder = 2, ScriptType = ScriptType.RunOnce });

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            upgradeBuilder.WithScriptsFromFileSystem(Path.Combine(scriptFolderPath, "SampleData"), new SqlScriptOptions { RunGroupOrder = 3, ScriptType = ScriptType.RunOnce });
        }
        var upgrader = upgradeBuilder.LogToConsole().Build();
        upgrader.PerformUpgrade();
    }
}