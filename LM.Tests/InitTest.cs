using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LM.Data;

namespace LM.Tests
{
    /// <summary>
    /// Общий код для всех тестов
    /// </summary>
    public static class TestsInit
    {
        private static ServiceProvider provider;

        public static IConfiguration configuration;

        public static ServiceProvider CreateService()
        {
            if (configuration == null)
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();
            }

            if (provider == null)
            {
                ServiceCollection services = new ServiceCollection();

                services.AddDataService(configuration);

                services.AddLogging(configure => configure.AddDebug());

                provider = services.BuildServiceProvider();
            }

            return provider;
        }
    }
}
