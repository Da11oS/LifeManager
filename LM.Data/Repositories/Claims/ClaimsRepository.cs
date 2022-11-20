using System.Security.Claims;
using DataModel;
using LinqToDB;

namespace LM.Data;

public class ClaimsRepository : Repository<AdmSchema.Claim>, IClaimsRepository
{
    public ClaimsRepository(LifeManagerDb context) : base(context)
    {
    }

    public async Task<IList<AdmSchema.Claim>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var res = await _ctx.Adm.Claims.Where(w => w.FUserId == userId).ToListAsync(cancellationToken); 
        return res;
    }

    public async Task<IList<AdmSchema.User>> GetUsersForClaimsAsync(string claimType, CancellationToken cancellationToken = default)
    {
        var res = await _ctx.Adm.Claims
            .Where(w => w.CKey == claimType)
            .Select(s => s.FUser)
            .ToListAsync(cancellationToken); 
        return res;
    }
}