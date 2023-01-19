using System.Text;
using LM.Api;
using LM.Api.Admin;
using LM.Base.Models;
using LM.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration
                            .AddJsonFile("appsettings.json", 
                                            optional: false, 
                                            reloadOnChange: true)
                            .Build();


// Add services to the container.
var services = builder.Services;

 var authsSection = Configuration.GetSection(nameof(Auths)).Get<Auths>();
//services.Configure<Auths>(authsSection);
services.AddSingleton(authsSection);
services.AddControllers();
services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDataService(Configuration.GetConnectionString("ConnectionStringLifeManager"));
services.AddScoped<IJwtService, JwtService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IClaimsRepository, ClaimsRepository>();

services.AddIdentityCore<UserModel>(opt =>
    {
        opt.Password.RequiredLength = 1;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireDigit = false;

        opt.User.AllowedUserNameCharacters = string.Empty;

    })
    .AddUserStore<UserService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPasswordService, PasswordService>();
services.AddScoped<IAuthorizationService, AuthorizationService>();

var key = new SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(authsSection?.TokenKey));

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateAudience = false,
                ValidateIssuer = false,
            };
        });	

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
