using System.Security.Claims;
using LM.Base.Models;

namespace LM.Api.Admin;

public interface IAuthorizationService
{
    public Task<RegisterResult> RegisterAsync(string mail, string name, string password, CancellationToken cancellationToken);
    public Task<LoginResult> LoginAsync(string mail, string password, CancellationToken cancellationToken);
    public Task<LogOutResult> LogOutAsync(string mail, string password, CancellationToken cancellationToken);
}

public record RegisterResult
{
    public string Error;
    public string AccessToken;
    public string RefreshToken;
    public Claim[] Claims;
}

public record LoginResult
{
    public string Error;
    public Claim[] Claims;
    public string AccessToken;
    public string RefreshToken;

}

public record LogOutResult
{
    
}


