using System.Security.Claims;
using LinqToDB;

namespace LM.Data;

public class ClaimsRepository : Repository<claims>, IClaimsRepository
{
    public ClaimsRepository(DbContext context) : base(context)
    {
    }

    public async Task<IList<claims>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var res = await _ctx.claims.Where(w => w.f_user_id == userId).ToListAsync(cancellationToken); 
        return res;
    }

    public async Task<IList<user>> GetUsersForClaimsAsync(string claimType, CancellationToken cancellationToken = default)
    {
        var res = await _ctx.claims
            .Where(w => w.c_key == claimType)
            .Select(s => s.f_user)
            .ToListAsync(cancellationToken); 
        return res;
    }
}