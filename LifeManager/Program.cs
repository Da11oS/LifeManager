using LM.Api.Admin;
using LM.Base.Models;
using LM.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
// Add services to the container.
var services = builder.Services;

services.AddControllers();
services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
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
    .AddUserStore<UserService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPasswordService, PasswordService>();
services.AddScoped<IAuthorizationService, AuthorizationService>();

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
