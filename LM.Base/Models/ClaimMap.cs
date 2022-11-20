using System.Security.Claims;
using DataModel;
using LM.Data;

namespace LM.Api.Views;

public static class ClaimMap
{
    public static Claim Map(this AdmSchema.Claim model)
    {
        return new Claim(model.CKey, model.CValue);
    }
    
    public static AdmSchema.Claim Map(this Claim model)
    {
        throw new NotImplementedException();
        return new AdmSchema.Claim
        {
            Id = default,
            FUserId = null,
            CKey = null,
            CValue = null,
            FUser = null
        };
    }
}