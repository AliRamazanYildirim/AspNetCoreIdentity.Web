using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.Anforderungen
{
    public class GewaltAnforderung: IAuthorizationRequirement
    {
        public int? Alter { get; set; }
    }
    public class GewaltAnforderungHandler : AuthorizationHandler<GewaltAnforderung>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GewaltAnforderung requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Geburtsdatum"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim geburtsdatumClaim = context.User.FindFirst("Geburtsdatum")!;
            var heute = DateTime.Now;
            var geburtsdatum = Convert.ToDateTime(geburtsdatumClaim.Value);
            var alter = heute.Year - geburtsdatum.Year;

            if (geburtsdatum > heute.AddYears(-alter)) alter--;

            if (requirement.Alter > alter)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
