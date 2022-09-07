using System.Security.Claims;
using System.Security.Cryptography;
using LM.Api.Views;
using LM.Base.Models;
using LM.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace LM.Api.Admin;

public class UserService : IUserService
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly IUserRepository _usersRepository;
    public UserService(IClaimsRepository repository, IUserRepository usersRepository)
    {
        _claimsRepository = repository;
        _usersRepository = usersRepository;
    }
    
    public void Dispose()
    {
        // ignore
    }

    #region user
    public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public async Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
    {
        await _usersRepository.SaveAsync((user with {UserName = userName}).Map(), cancellationToken);
    }

    public Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
    {
       return Task.FromResult(user.NormalizedUserName);
    }

    public async Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
    {
        await _usersRepository.SaveAsync((user with { NormalizedUserName = normalizedName}).Map(), cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        
        try
        {
            await _usersRepository.SaveAsync(user.Map(), cancellationToken);
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

    public async Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
    {
        try
        {
            await _usersRepository.SaveAsync(user.Map(), cancellationToken);
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
    
    public async Task<IdentityResult> DeleteAsync(UserModel user, CancellationToken cancellationToken)
    {
        try
        {
            await _usersRepository.SaveAsync(user.Map(), cancellationToken);
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

    public async Task<UserModel> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.id.ToString() == userId, cancellationToken);
        
        return res?.Map();
    }


    public async Task<UserModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.normalize_name == normalizedUserName, cancellationToken);
        
        return res?.Map();
    }
    
    public async Task<UserModel> FindByMailAsync(string mail, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.mail == mail, cancellationToken);
        
        return res?.Map();
    }

    public async Task<UserModel> FindByNameOrMailAsync(string normalizedName, string mail, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.mail == mail || u.normalize_name == normalizedName, cancellationToken);
        
        return res?.Map();    
    }

    #endregion

    #region claims

    public async Task<IList<Claim>> GetClaimsAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var res = await _claimsRepository.FindByUserIdAsync(user.Id, cancellationToken); 
        return res.Select(s => s.Map()).ToList();
    }

    public Task AddClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        var items = claims.Select(s => new claims
        {
            id = default,
            f_user_id = user.Id,
            c_key = s.Type,
            c_value = s.Value,
        });
        return _claimsRepository.SaveManyAsync(items, cancellationToken);
    }

    public async Task ReplaceClaimAsync(UserModel user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        var dbclaim = await _claimsRepository
            .Get(g => g.f_user_id == user.Id 
                      && g.c_key == claim.Type,cancellationToken);

        await _claimsRepository.SaveAsync(new claims
        {
            id = dbclaim.id,
            f_user_id = dbclaim.id,
            c_key = newClaim.Type,
            c_value = newClaim.Value,
        }, cancellationToken);
    }

    public Task RemoveClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        var claimsTypes = claims.Select(s => s.Type);
        return _claimsRepository
            .DeleteAsync(d => d.f_user_id == user.Id 
                              && claimsTypes.Contains(d.c_key)
                , cancellationToken);
    }

    public async Task<IList<UserModel>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        var res = await _claimsRepository.GetUsersForClaimsAsync(claim.Type, cancellationToken);
        return res.Select(s => s.Map()).ToList();
    }

    #endregion
    
}

