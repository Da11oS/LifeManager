using Database;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Common;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LM.Data;
public static class DataServiceExtension
{
    /// <summary>
    /// Добавить сервисы данных
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns></returns>
    public static IServiceCollection AddDataService(this IServiceCollection services, IConfiguration config)
    {
        //services.AddScoped<IIdentityUserSvc, IdentityUserSvc>();

        services.AddLinqToDBContext<DbContext>((provider, options) => {
            options
                .UsePostgreSQL(config.GetConnectionString("ConnectionStringLifeManager"))
                .UseDefaultLogging(provider);
        });

        return services;
    }
}
