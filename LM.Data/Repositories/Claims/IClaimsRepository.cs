using System.Security.Claims;

namespace LM.Data;

public interface IClaimsRepository : IRepository<claims>
{
    public Task<IList<claims>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<IList<user>> GetUsersForClaimsAsync(string claimType, CancellationToken cancellationToken = default);

}