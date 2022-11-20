
namespace LM.Data;

public interface IUserRepository : IRepository<user>
{
    public Task<user?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    
}