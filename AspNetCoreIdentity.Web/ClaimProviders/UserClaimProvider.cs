using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppBenutzer> _userManager;

        public UserClaimProvider(UserManager<AppBenutzer> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identitaet = principal.Identity as ClaimsIdentity;
            var aktuellerBenutzer = await _userManager.FindByNameAsync(identitaet!.Name!);

            if (String.IsNullOrEmpty(aktuellerBenutzer!.Stadt))
            {
                return principal;
            }
            if (principal.HasClaim(x => x.Type != "stadt"))
            {
                Claim claim = new("Stadt", aktuellerBenutzer!.Stadt!);
                identitaet.AddClaim(claim);
            }
            return principal;
        }
    }
}
