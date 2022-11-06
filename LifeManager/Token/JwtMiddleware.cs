using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LM.Base.Admin;
using Microsoft.IdentityModel.Tokens;

namespace LM.Api.Admin;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        this._next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();
        try
        {
            AttachUserToContext(context, userService, token);
        }
        finally
        {
            await _next.Invoke(context);
        }
    }

    public void AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // min 16 characters
            var key = Encoding.ASCII.GetBytes(_configuration["AuthKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var userMail = jwtToken.Claims.First(x => x.Type == CustomClaimsType.Mail).Value;

            context.Items["User"] = userService.FindByMailAsync(userMail).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}