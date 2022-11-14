using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LM.Base.Models;
using LM.Data;
using LM.Data.RefreshKeys;
using Microsoft.IdentityModel.Tokens;

namespace LM.Api.Admin;

public class JwtService : IJwtService
{
    private readonly Auths _authsConfig;
    private readonly SymmetricSecurityKey _key;
    private readonly IRefreshKeysRepository _refreshKeysRepository;

    public JwtService(IConfiguration config, Auths authsConfig, IRefreshKeysRepository refreshKeysRepository)
    {
        _authsConfig = authsConfig;
        _refreshKeysRepository = refreshKeysRepository;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authsConfig.TokenKey));
    }

    public string CreateAccessToken(UserModel user)
    {
        var claims = new List<Claim> {new(JwtRegisteredClaimNames.NameId, user.UserName)};

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMilliseconds(_authsConfig.AccessLifeTimeMs),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public Task<refresh_keys?> GetRefreshToken(UserModel user)
    {
        return _refreshKeysRepository
            .Get((key) => key.f_user_id == user.Id);
    }

    public async Task<refresh_keys?> UpdateRefreshTokenAsync(refresh_keys token,
        CancellationToken cancellationToken = default)
    {
        var key = Guid.NewGuid();
        var newToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key.ToString()));
        var updatedKey = new refresh_keys
        {
            id = token.id,
            key = newToken,
            n_expires = DateTime.Now.AddMilliseconds(_authsConfig.RefreshLifeTimeMs),
            f_user_id = token.f_user_id
        };
        
        await _refreshKeysRepository.SaveAsync(updatedKey);
        
        return updatedKey;
    }

    public Task<bool> IsValidRefreshToken(string refreshToken, Guid userId, CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.Now;
        return _refreshKeysRepository
            .AnyAsync((key) => key.key == refreshToken 
                               && key.f_user_id == userId
                && key.n_expires <= now, cancellationToken);
    }
    
    
}