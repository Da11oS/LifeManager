using LM.Base.Models;
using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public interface IPasswordService : IPasswordHasher<UserModel>
{
    
}