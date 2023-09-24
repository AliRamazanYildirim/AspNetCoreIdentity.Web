using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentity.Web.SamenDaten
{
    public class BerechtigungSamenDaten
    {
        public static async Task Samen(RoleManager<AppRolle> roleManager)
        {
            var gibtsBasisRolle = await roleManager.RoleExistsAsync("BasisRolle");
            if (!gibtsBasisRolle)
            {
                await roleManager.CreateAsync(new AppRolle()
                {
                    Name = "BasisRolle"
                });
                var basisRolle = await roleManager.FindByNameAsync("BasisRolle");
                await roleManager.AddClaimAsync(basisRolle!, new Claim
                    ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Vorrat.Lesen));
                await roleManager.AddClaimAsync(basisRolle!, new Claim
                   ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Bestellung.Lesen));
                await roleManager.AddClaimAsync(basisRolle!, new Claim
                   ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Katalog.Lesen));
            }
        }
    }
}
