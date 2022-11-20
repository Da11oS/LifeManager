using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LM.Base.Admin;
using Microsoft.IdentityModel.Tokens;

namespace LM.Api.Admin;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;
    private readonly Auths _auths;
    public JwtMiddleware(RequestDelegate next, IJwtService jwtService, IUserService userService, Auths auths)
    {
        this._next = next;
        _jwtService = jwtService;
        _userService = userService;
        _auths = auths;
    }

    public async Task InvokeAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();
        var refreshToken = context.Request.Headers["refresh-key"]
            .FirstOrDefault();
        
        try
        {
            var valideatedAccessToken = GetValidatedToken(token);
            var now = DateTime.Now;
            var accesTokenDiff = now - valideatedAccessToken.ValidTo;
            var jwtToken = (JwtSecurityToken) valideatedAccessToken;
            var userMail = jwtToken.Claims.First(x => x.Type == CustomClaimsType.Mail).Value;
            var user = await _userService.FindByMailAsync(userMail, cancellationToken);
            var isValidRefreshToken = await _jwtService.IsValidRefreshToken(refreshToken ?? "", user.Id, cancellationToken);
            if (accesTokenDiff.Seconds <= 0 &&  user != null && isValidRefreshToken)
            {
                var newAccessToken = await GetNewAccessTokenAsync(valideatedAccessToken);
                var currentUserToken = await  _jwtService.GetRefreshToken(user);
                var newRefreshToken = await _jwtService.UpdateRefreshTokenAsync(currentUserToken, cancellationToken);
                context.Response.Headers.Append("Authorization", newAccessToken); 
                context.Response.Headers.Append("refresh-key", newRefreshToken?.Key);
            }
            else
            {
                throw new NotImplementedException("Необходимо пройти повторную авторизацию");
            }
            context.Items["User"] = user;
        }
        finally
        {
            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private async Task<string> GetNewAccessTokenAsync(SecurityToken validatedToken)
    {
        var jwtToken = (JwtSecurityToken) validatedToken;
        var userMail = jwtToken.Claims.First(x => x.Type == CustomClaimsType.Mail).Value;
        var user = await _userService.FindByMailAsync(userMail);
        return _jwtService.CreateAccessToken(user);
    }

    private SecurityToken? GetValidatedToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key =   Encoding.ASCII.GetBytes(_auths.TokenKey);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        return validatedToken;
    }
}