using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public interface IPasswordService : IPasswordHasher<UserView>
{
    
}