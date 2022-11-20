using System.Security.Claims;
using DataModel;

namespace LM.Data;

public interface IClaimsRepository : IRepository<AdmSchema.Claim>
{
    public Task<IList<AdmSchema.Claim>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<IList<AdmSchema.User>> GetUsersForClaimsAsync(string claimType, CancellationToken cancellationToken = default);

}