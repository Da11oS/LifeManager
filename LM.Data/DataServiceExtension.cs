using LinqToDB.Configuration;
using Microsoft.AspNetCore.Identity;
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
        services.AddScoped<LM.Data.DbContext>();
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddUserStore<IdentityUser<Guid>>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IUserStore<IdentityUser<Guid>>, User>();
        // services.AddTransient<IRoleStore<IdentityRole>, FakeRoleStore>();
        var polyConnectionOptBuilder = new LinqToDBConnectionOptionsBuilder()
            .UsePostgreSQL(config.GetConnectionString("ConnectionStringLifeManager"));

        var polyConnectionOptions = new LinqToDBConnectionOptions(polyConnectionOptBuilder);

        LM.Data.DbContextOptions dbContextOptions = new(polyConnectionOptions);

        services.AddSingleton(dbContextOptions);

        return services;
    }
}
