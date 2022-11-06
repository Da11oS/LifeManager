using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LM.Base.Admin;
using LM.Base.Models;
using LM.Data;
using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly DbContext _ctx;

    public AuthorizationService(IPasswordService passwordService, IUserService userService, DbContext ctx, IJwtService jwtService)
    {
        _passwordService = passwordService;
        _userService = userService;
        _ctx = ctx;
        _jwtService = jwtService;
    }

    public async Task<RegisterResult> RegisterAsync(string mail, string name, string password,
        CancellationToken cancellationToken)
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

        var tr = await _ctx.BeginTransactionAsync(cancellationToken);
        try
        {
            // TODO: Надо проверить меняет ли объект user внутри
            var hashedPassword = _passwordService.HashPassword(null, password);
            var newGuid = Guid.NewGuid();
            var newUser = new UserModel
            {
                Id = newGuid,
                Email = mail,
                PasswordHash = hashedPassword,
                UserName = name,
                NormalizedUserName = name.ToLower()
            };

            var res = await _userService.CreateAsync(newUser, cancellationToken);
            if (!res.Succeeded)
            {
                throw new NotImplementedException(String.Join("", res.Errors.Select(s => s.Description), "\n"));
            }
            var refreshToken = _jwtService.CreateRefreshToken(newUser); 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, newUser.UserName),
                new Claim(CustomClaimsType.RefreshToken, refreshToken),
            };
            await _userService.AddClaimsAsync(newUser,
                claims, cancellationToken);

            tr.CommitAsync(cancellationToken);

            var userClaims = await _userService.GetClaimsAsync(newUser, cancellationToken);
            var jwt = _jwtService.CreateAccessToken(newUser); 
            return new RegisterResult()
            {
                Claims = userClaims.ToArray(),
                
            };
        }
        catch (Exception e)
        {
            tr.RollbackAsync(cancellationToken);
            return new RegisterResult()
            {
                Error = e.Message,
            };
        }
    }

    public async Task<LoginResult> LoginAsync(string mail, string password, CancellationToken cancellationToken)
    {
        var user = await _userService.FindByMailAsync(mail, cancellationToken);
        if (user == null)
            throw new NotImplementedException($"Пользователя с почтой {mail} не существует!");

        if (_passwordService.VerifyHashedPassword(user, user.PasswordHash, password)
            == PasswordVerificationResult.Success)
        {
            var userClaims = await _userService.GetClaimsAsync(user, cancellationToken);
            var jwt = _jwtService.CreateAccessToken(user);
            return new LoginResult()
            {
                Claims = userClaims.ToArray(),
                AccessToken = jwt

            };
        }

        return new LoginResult()
        {
            Error = "Не верно введен пароль!"
        };
    }

    public Task<LogOutResult> LogOutAsync(string mail, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}