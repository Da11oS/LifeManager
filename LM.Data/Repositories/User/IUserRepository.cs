
using DataModel;

namespace LM.Data;

public interface IUserRepository : IRepository<AdmSchema.User>
{
    public Task<AdmSchema.User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    
}