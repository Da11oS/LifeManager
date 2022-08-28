using LinqToDB.Configuration;
namespace LM.Data;

public class DbContextOptions
{
    internal string PolyConnectionString { get; }
    internal LinqToDBConnectionOptions PolyConnectionOptions { get; }
    internal DbContextOptions(LinqToDBConnectionOptions connectionOptions)
    {
        PolyConnectionString = connectionOptions.ConnectionString ??
                               throw new ArgumentNullException(nameof(connectionOptions));
        PolyConnectionOptions = connectionOptions;
    }
}