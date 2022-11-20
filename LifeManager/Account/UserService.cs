using System.Security.Claims;
using System.Security.Cryptography;
using DataModel;
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
        var res = await _usersRepository.Get((u) => u.Id.ToString() == userId, cancellationToken);
        
        return res?.Map();
    }


    public async Task<UserModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.NormalizeName == normalizedUserName, cancellationToken);
        
        return res?.Map();
    }
    
    public async Task<UserModel> FindByMailAsync(string mail, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.Mail == mail, cancellationToken);
        
        return res?.Map();
    }

    public async Task<UserModel> FindByNameOrMailAsync(string normalizedName, string mail, CancellationToken cancellationToken)
    {
        var res = await _usersRepository.Get((u) => u.Mail == mail || u.NormalizeName == normalizedName, cancellationToken);
        
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
        var items = claims.Select(s => new AdmSchema.Claim()
        {
            Id = default,
            FUserId = user.Id,
            CKey = s.Type,
            CValue = s.Value,
        });
        return _claimsRepository.SaveManyAsync(items, cancellationToken);
    }

    public async Task ReplaceClaimAsync(UserModel user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
    {
        var dbclaim = await _claimsRepository
            .Get(g => g.FUserId == user.Id 
                      && g.CKey == claim.Type,cancellationToken);

        await _claimsRepository.SaveAsync(new AdmSchema.Claim()
        {
            Id = dbclaim.Id,
            FUserId = dbclaim.Id,
            CKey = newClaim.Type,
            CValue = newClaim.Value,
        }, cancellationToken);
    }

    public Task RemoveClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
    {
        var claimsTypes = claims.Select(s => s.Type);
        return _claimsRepository
            .DeleteAsync(d => d.FUserId == user.Id 
                              && claimsTypes.Contains(d.CKey)
                , cancellationToken);
    }

    public async Task<IList<UserModel>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
    {
        var res = await _claimsRepository.GetUsersForClaimsAsync(claim.Type, cancellationToken);
        return res.Select(s => s.Map()).ToList();
    }

    #endregion
    
}

