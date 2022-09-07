using LM.Base.Models;
using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public class PasswordService : IPasswordService
{
    public string HashPassword(UserModel user, string password)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        if(user != null)
            user.PasswordHash = hashed;
        return hashed;
    }

    public PasswordVerificationResult VerifyHashedPassword(UserModel user, string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword) 
            ? PasswordVerificationResult.Success 
            : PasswordVerificationResult.Failed;
    }
}