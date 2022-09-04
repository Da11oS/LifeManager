using System.Security.Claims;
using LM.Data;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace LM.Api.Admin;

public class UserService : IUserService
{
    private readonly IUserRepository _repository; 
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public void Dispose()
    {
        // ignore
    }

    public Task<string> GetUserIdAsync(UserView user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(UserView user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public async Task SetUserNameAsync(UserView user, string userName, CancellationToken cancellationToken)
    {
        await _repository.SaveAsync((user with {UserName = userName}).Map(), cancellationToken);
    }

    public Task<string> GetNormalizedUserNameAsync(UserView user, CancellationToken cancellationToken)
    {
       return Task.FromResult(user.NormalizedUserName);
    }

    public async Task SetNormalizedUserNameAsync(UserView user, string normalizedName, CancellationToken cancellationToken)
    {
        await _repository.SaveAsync((user with { NormalizedUserName = normalizedName}).Map(), cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(UserView user, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveAsync(user.Map(), cancellationToken);
        }
        catch (PostgresException ex)
        {
            return IdentityResult.Failed(new IdentityError()
            {
                Code = ex.ErrorCode.ToString(),
                Description = ex.Detail
            });
        }
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(UserView user, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveAsync(user.Map(), cancellationToken);
        }
        catch (PostgresException ex)
        {
            return IdentityResult.Failed(new IdentityError()
            {
                Code = ex.ErrorCode.ToString(),
                Description = ex.Detail
            });
        }
        
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(UserView user, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveAsync(user.Map(), cancellationToken);
        }
        catch (PostgresException ex)
        {
            return IdentityResult.Failed(new IdentityError()
            {
                Code = ex.ErrorCode.ToString(),
                Description = ex.Detail
            });
        }
        
        return IdentityResult.Success;
    }

    public async Task<UserView> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var res = await _repository.Get((u) => u.id.ToString() == userId, cancellationToken);
        
        return res.Map();
    }


    public async Task<UserView> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var res = await _repository.Get((u) => u.normalize_name == normalizedUserName, cancellationToken);
        
        return res.Map();
    }

    public Task<IList<Claim>> GetClaimsAsync(UserView user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task AddClaimsAsync(UserView user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ReplaceClaimAsync(UserView user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveClaimsAsync(UserView user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<UserView>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPasswordHashAsync(UserView user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(UserView user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
        
    }

    public Task<bool> HasPasswordAsync(UserView user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

