using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public interface IUserService : IUserClaimStore<UserView>
{
    public Task<UserView> FindByMailAsync(string mail, CancellationToken cancellationToken);
    
    public Task<UserView> FindByNameOrMailAsync(string normalizedName,string mail, CancellationToken cancellationToken);

}