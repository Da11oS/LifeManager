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
    public static IServiceCollection AddDataService(this IServiceCollection services, string connetctionString)
    {
        services.AddScoped<LM.Data.DbContext>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshKeysRepository, RefreshKeysRepository>();


        // services.AddTransient<IRoleStore<IdentityRole>, FakeRoleStore>();
        var polyConnectionOptBuilder = new LinqToDBConnectionOptionsBuilder()
            .UsePostgreSQL(connetctionString);

        var polyConnectionOptions = new LinqToDBConnectionOptions(polyConnectionOptBuilder);

        LM.Data.DbContextOptions dbContextOptions = new(polyConnectionOptions);

        services.AddSingleton(dbContextOptions);

        return services;
    }
}
