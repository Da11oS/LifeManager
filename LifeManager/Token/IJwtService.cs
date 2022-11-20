using System.Security.Claims;
using DataModel;
using LM.Base.Models;
using LM.Data;

namespace LM.Api.Admin;

public interface IJwtService
{
    string CreateAccessToken(UserModel user);
    Task<AdmSchema.RefreshKey?> GetRefreshToken(UserModel user);
    Task<bool> IsValidRefreshToken(string refreshToken, Guid userId,
        CancellationToken cancellationToken = default);

    Task<AdmSchema.RefreshKey?> UpdateRefreshTokenAsync(AdmSchema.RefreshKey token,
        CancellationToken cancellationToken = default);
}