using DataModel;
using LinqToDB.Configuration;
namespace LM.Data;

public class DbContextOptions
{
    internal string ConnectionString { get; }
    internal LinqToDBConnectionOptions<LifeManagerDb> PolyConnectionOptions { get; }
    internal DbContextOptions(LinqToDBConnectionOptions<LifeManagerDb> connectionOptions)
    {
        ConnectionString = connectionOptions.ConnectionString ??
                               throw new ArgumentNullException(nameof(connectionOptions));
        PolyConnectionOptions = connectionOptions;
    }
}