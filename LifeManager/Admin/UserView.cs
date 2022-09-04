using LM.Data;

namespace LM.Api.Admin;

public record UserView
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
};

public static class UserMap
{
    public static user Map(this UserView identity)
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
    
    public static UserView Map(this user user)
    {
        return new UserView
        {
            Id = user.id,
            Email = user.mail,
            NormalizedUserName = user.normalize_name,
            PasswordHash = user.password,
            UserName = user.user_name
        };
    }
}