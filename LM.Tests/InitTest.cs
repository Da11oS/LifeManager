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
        private static ServiceProvider _provider;

        public static IConfiguration Configuration;

        public static ServiceProvider CreateService()
        {
            if (Configuration == null)
            {
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();
            }

            if (_provider == null)
            {
                ServiceCollection services = new ServiceCollection();

                services.AddDataService(Configuration.GetConnectionString("ConnectionStringLifeManager"));

                services.AddLogging(configure => configure.AddDebug());

                _provider = services.BuildServiceProvider();
            }

            return _provider;
        }
    }
}
