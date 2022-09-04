using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    public AuthorizationService(IPasswordService passwordService, IUserService userService)
    {
        _passwordService = passwordService;
        _userService = userService;
    }

    public async Task<RegisterResult> RegisterAsync(string mail, string name, string password, CancellationToken cancellationToken)
    {
        var user = await _userService.FindByNameOrMailAsync(name.ToLower(), mail, cancellationToken);

        if (user != null)
        {
            if (user.Email == mail)
            {
                throw new NotImplementedException("Пользователь с такой почтой уже существует!");
            }

            if (user.UserName == name)
            {
                throw new NotImplementedException("Пользователь с таким именем уже существует!");
            }
        }

        try
        {
            // TODO: Надо проверить меняет ли объект user внутри
            var hashedPassword = _passwordService.HashPassword(null, password);
            var newGuid = Guid.NewGuid();
            await _userService.CreateAsync(new UserView
            {
                Id = newGuid,
                Email = mail,
                PasswordHash = hashedPassword,
                UserName = name,
                NormalizedUserName = name.ToLower()
            }, cancellationToken);

            return new RegisterResult()
            {
                Success = true
            };
        }
        catch (Exception e)
        {
            return new RegisterResult()
            {
                Error = e.Message,
                Success = false
            };
        }
    }

    public async Task<LogInResult> LogInAsync(string mail, string password, CancellationToken cancellationToken)
    {
        var user = await _userService.FindByMailAsync(mail, cancellationToken);
        if (user == null)
            throw new NotImplementedException($"Пользователя с почтой {mail} не существует!");
        
        if (_passwordService.VerifyHashedPassword(user, user.PasswordHash, password) 
            == PasswordVerificationResult.Success)
        {
            return new LogInResult()
            {
                Success = true,
                User = user,
            };
        }
        
        return new LogInResult()
        {
            Success = false,
            Error = "Не верно введен пароль!"
        };
    }

    public Task<LogOutResult> LogOutAsync(string mail, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}