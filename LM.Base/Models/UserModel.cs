using DataModel;
using LM.Data;

namespace LM.Base.Models;

public record UserModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
};

public static class UserMap
{
    public static AdmSchema.User Map(this UserModel identity)
    {
        return new AdmSchema.User
        {
            Id = identity.Id,
            Mail = identity.Email,
            NormalizeName = identity.NormalizedUserName,
            Password = identity.PasswordHash,
            UserName = identity.UserName
        };
    }
    
    public static UserModel Map(this AdmSchema.User user)
    {
        return new UserModel
        {
            Id = user.Id,
            Email = user.Mail,
            NormalizedUserName = user.NormalizeName,
            PasswordHash = user.Password,
            UserName = user.UserName
        };
    }
}