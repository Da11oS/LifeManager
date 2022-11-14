using System.Security.Claims;
using LM.Base.Models;
using LM.Data;

namespace LM.Api.Admin;

public interface IJwtService
{
    string CreateAccessToken(UserModel user);
    Task<refresh_keys?> GetRefreshToken(UserModel user);
    Task<bool> IsValidRefreshToken(string refreshToken, Guid userId,
        CancellationToken cancellationToken = default);

    Task<refresh_keys?> UpdateRefreshTokenAsync(refresh_keys token,
        CancellationToken cancellationToken = default);
}