using LM.Api.Admin;
using LM.Base.Models;
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
                services.AddIdentityCore<UserModel>(opt =>
                    {
                        opt.Password.RequiredLength = 1;
                        opt.Password.RequireNonAlphanumeric = false;
                        opt.Password.RequireLowercase = false;
                        opt.Password.RequireUppercase = false;
                        opt.Password.RequireDigit = false;

                        opt.User.AllowedUserNameCharacters = string.Empty;

                    })
                    .AddUserStore<UserService>()
                    .AddPasswordValidator<UserService>();
                services.AddSingleton(Configuration);
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IPasswordService, PasswordService>();
                services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IClaimsRepository, ClaimsRepository>();
                services.AddScoped<IJwtService, JwtService>();

            _provider = services.BuildServiceProvider();
            }

            return _provider;
        }
    }
}
