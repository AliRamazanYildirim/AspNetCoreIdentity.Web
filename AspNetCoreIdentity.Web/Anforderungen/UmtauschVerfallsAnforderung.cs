using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.Anforderungen
{
    public class UmtauschVerfallsAnforderung:IAuthorizationRequirement
    {
    }

    public class UmtauschVerfallsAnforderungHandler : AuthorizationHandler<UmtauschVerfallsAnforderung>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UmtauschVerfallsAnforderung requirement)
        {
            if(!context.User.HasClaim(x => x.Type == "AblaufDatumDesUmtauschs"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim umtauschClaimAblaufdatum = context.User.FindFirst( "AblaufDatumDesUmtauschs")!;
            if (DateTime.Now > Convert.ToDateTime(umtauschClaimAblaufdatum.Value))
            {
                context.Fail(); 
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;

        }
    }
}
