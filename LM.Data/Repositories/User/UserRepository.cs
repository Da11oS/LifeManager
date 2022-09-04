using System.Linq.Expressions;
using LinqToDB;
using Microsoft.AspNetCore.Identity;

namespace LM.Data;

public class UserRepository : Repository<user>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public Task<user?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _ctx.user.FirstOrDefaultAsync(f => f.normalize_name == normalizedUserName, cancellationToken);
    }
    
}