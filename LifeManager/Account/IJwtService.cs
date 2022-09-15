using System.Security.Claims;
using LM.Base.Models;

namespace LM.Api.Admin;

public interface IJwtService
{
    string CreateAccessToken(UserModel user, int lifeTime = 1);
    string CreateRefreshToken(UserModel user, int lifeTime = 1);
}