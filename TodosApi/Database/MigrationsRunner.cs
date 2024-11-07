using DbUp;

namespace TodosApi.Database;

public sealed class MigrationsRunner(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void Run()
    {
        var connectionString = _configuration.GetConnectionString("Default");
        var upgrader = DeployChanges
            .To.SQLiteDatabase(connectionString)
            .WithScriptsFromFileSystem("./Database/Scripts")
            .LogToConsole()
            .Build();

        if (upgrader.IsUpgradeRequired())
        {
            upgrader.PerformUpgrade();
        }
    }
}
