using System.Security.Claims;
using LM.Base.Models;
using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public interface IUserService : IUserClaimStore<UserModel>
{
    public Task<UserModel> FindByMailAsync(string mail, CancellationToken cancellationToken);
    
    public Task<UserModel> FindByNameOrMailAsync(string normalizedName,string mail, CancellationToken cancellationToken);

}