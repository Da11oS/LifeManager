using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace LM.Data;

public partial class User: IUserStore<IdentityUser<Guid>>
{
    private DbContext _ctx;
    
    public User(DbContext ctx)
    {
        _ctx = ctx;
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserIdAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        var res = await _ctx.user
            .FirstAsync(f => f.id== user.Id, cancellationToken);
        return res.id.ToString();
    }

    public async Task<string> GetUserNameAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        var res = await _ctx.user
            .FirstAsync(f => f.id == user.Id, cancellationToken);
        
        return res.user_name;
    }

    public async Task SetUserNameAsync(IdentityUser<Guid> user, string userName, CancellationToken cancellationToken)
    {
        await _ctx.user
            .Where(w => w.id== user.Id)
            .Set(s => s.user_name, v => userName)
            .UpdateAsync(cancellationToken);
    }

    public async Task<string> GetNormalizedUserNameAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        var res = await _ctx.user
            .FirstAsync(f => f.id == user.Id, cancellationToken);
        
        return res.normalize_name;
    }

    public async Task SetNormalizedUserNameAsync(IdentityUser<Guid> user, string normalizedName, CancellationToken cancellationToken)
    {
        await _ctx.user
            .Where(w => w.id == user.Id)
            .Set(s => s.normalize_name, v => normalizedName)
            .UpdateAsync(cancellationToken);
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _ctx.InsertAsync<user>(user.Map(), token: cancellationToken);
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

    public async Task<IdentityResult> UpdateAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _ctx.UpdateAsync<user>(user.Map(), token: cancellationToken);
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

    public async Task<IdentityResult> DeleteAsync(IdentityUser<Guid> user, CancellationToken cancellationToken)
    {
        try
        {
            var res = await _ctx.DeleteAsync<user>(user.Map(), token: cancellationToken);
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
    
    public Task<IdentityUser<Guid>> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return _ctx.user
            .Where(f => f.id.ToString() == userId)
            .Select(s => s.Map())
            .FirstAsync(cancellationToken);
    }

    public Task<IdentityUser<Guid>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _ctx.user
            .Where(f => f.normalize_name == normalizedUserName)
            .Select(s => s.Map())
            .FirstAsync(cancellationToken);    }
}

public static class UserMap
{
    public static user Map(this IdentityUser<Guid> identity)
    {
        return new user
        {
            id = identity.Id,
            mail = identity.Email,
            normalize_name = identity.NormalizedUserName,
            password = identity.PasswordHash,
            user_name = identity.UserName
        };
    }
    
    public static IdentityUser<Guid> Map(this user user)
    {
        return new IdentityUser<Guid>
        {
            Id = user.id,
            Email = user.mail,
            NormalizedUserName = user.normalize_name,
            PasswordHash = user.password,
            UserName = user.user_name
        };
    }
}