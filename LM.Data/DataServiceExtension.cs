using DataModel;
using LinqToDB.Configuration;
using LM.Data.RefreshKeys;
using Microsoft.Extensions.DependencyInjection;

namespace LM.Data;
public static class DataServiceExtension
{
    /// <summary>
    /// Добавить сервисы данных
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddDataService(this IServiceCollection services, string connectionString)
    {
        
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshKeysRepository, RefreshKeysRepository>();


        var polyConnectionOptBuilder = new LinqToDBConnectionOptionsBuilder()
            .UsePostgreSQL(connectionString);

        var polyConnectionOptions = new LinqToDBConnectionOptions<LifeManagerDb>(polyConnectionOptBuilder);
        
        services.AddSingleton(polyConnectionOptions);
        
        services.AddScoped<LifeManagerDb>();
        
        return services;
    }
}
