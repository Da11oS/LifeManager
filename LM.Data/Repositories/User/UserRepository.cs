using DataModel;
using LinqToDB;

namespace LM.Data;

public class UserRepository : Repository<AdmSchema.User>, IUserRepository
{
    public UserRepository(LifeManagerDb context) : base(context)
    {
    }

    public Task<AdmSchema.User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return _ctx.Adm.Users.FirstOrDefaultAsync(f => f.NormalizeName == normalizedUserName, cancellationToken);
    }
    
}