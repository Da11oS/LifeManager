using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LM.Base.Models;
using Microsoft.IdentityModel.Tokens;

namespace LM.Api.Admin;

public class JwtService : IJwtService
{
    private readonly SymmetricSecurityKey _key;

    public JwtService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }

    public string CreateAccessToken(UserModel user, int lifeTime = 1)
    {
        var claims = new List<Claim> {new(JwtRegisteredClaimNames.NameId, user.UserName)};

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(lifeTime),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken(UserModel user, int lifeTime = 1)
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}