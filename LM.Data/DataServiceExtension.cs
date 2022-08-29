﻿using LinqToDB.Configuration;
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
        var polyConnectionOptBuilder = new LinqToDBConnectionOptionsBuilder()
            .UsePostgreSQL(config.GetConnectionString("ConnectionStringLifeManager"));

        var polyConnectionOptions = new LinqToDBConnectionOptions(polyConnectionOptBuilder);

        LM.Data.DbContextOptions dbContextOptions = new(polyConnectionOptions);

        services.AddSingleton(dbContextOptions);

        return services;
    }
}
