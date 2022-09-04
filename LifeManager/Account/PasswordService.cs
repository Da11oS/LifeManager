using Microsoft.AspNetCore.Identity;

namespace LM.Api.Admin;

public class PasswordService : IPasswordService
{
    public string HashPassword(UserView user, string password)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        user.PasswordHash = hashed;;
        return hashed;
    }

    public PasswordVerificationResult VerifyHashedPassword(UserView user, string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword) 
            ? PasswordVerificationResult.Success 
            : PasswordVerificationResult.Failed;
    }
}