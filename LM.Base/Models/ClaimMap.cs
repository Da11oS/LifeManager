using System.Security.Claims;
using LM.Data;

namespace LM.Api.Views;

public static class ClaimMap
{
    public static Claim Map(this claims model)
    {
        return new Claim(model.c_key, model.c_value);
    }
    
    public static claims Map(this Claim model)
    {
        throw new NotImplementedException();
        return new claims
        {
            id = default,
            f_user_id = null,
            c_key = null,
            c_value = null,
            f_user = null
        };
    }
}