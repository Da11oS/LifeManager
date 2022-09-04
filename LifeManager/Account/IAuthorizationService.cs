namespace LM.Api.Admin;

public interface IAuthorizationService
{
    public Task<RegisterResult> RegisterAsync(string mail, string name, string password, CancellationToken cancellationToken);
    public Task<LogInResult> LogInAsync(string mail, string password, CancellationToken cancellationToken);
    public Task<LogOutResult> LogOutAsync(string mail, string password, CancellationToken cancellationToken);
}

public record RegisterResult
{
    public string Error;
    public string JwtToken;
    public bool Success;
    public UserView User;
}

public record LogInResult
{
    public string Error;
    public bool Success;
    public UserView User;
}

public record LogOutResult
{
    
}


