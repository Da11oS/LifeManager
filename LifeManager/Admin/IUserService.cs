using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public interface IUserService : IUserClaimStore<UserView>, IUserPasswordStore<UserView>
{
    
}