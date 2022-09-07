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
    public static user Map(this UserModel identity)
    {
        return new user
        {
            id = identity.Id,
            mail = identity.Email,
            normalize_name = identity.NormalizedUserName,
            password = identity.PasswordHash,
            user_name = identity.UserName
        };
    }
    
    public static UserModel Map(this user user)
    {
        return new UserModel
        {
            Id = user.id,
            Email = user.mail,
            NormalizedUserName = user.normalize_name,
            PasswordHash = user.password,
            UserName = user.user_name
        };
    }
}