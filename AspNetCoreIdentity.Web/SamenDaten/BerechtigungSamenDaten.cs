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
            var gibtsFortgeschritteneRolle = await roleManager.RoleExistsAsync("FortgeschritteneRolle");
            var gibtsAdminRolle = await roleManager.RoleExistsAsync("AdminRolle");


            if (!gibtsBasisRolle)
            {
                await roleManager.CreateAsync(new AppRolle()
                {
                    Name = "BasisRolle"
                });
                var basisRolle = await roleManager.FindByNameAsync("BasisRolle");
                await LesenBerechtigung(basisRolle!, roleManager);
            }
            if (!gibtsFortgeschritteneRolle)
            {
                await roleManager.CreateAsync(new AppRolle()
                {
                    Name = "FortgeschritteneRolle"
                });
                var fortgeschritteneRolle = await roleManager.FindByNameAsync("FortgeschritteneRolle");
                await LesenBerechtigung(fortgeschritteneRolle!, roleManager);
                await ErstellenUndAktualisierenBerechtigung(fortgeschritteneRolle!, roleManager);

            }
            if (!gibtsAdminRolle)
            {
                await roleManager.CreateAsync(new AppRolle()
                {
                    Name = "AdminRolle"
                });
                var adminRolle = await roleManager.FindByNameAsync("AdminRolle");
                await LesenBerechtigung(adminRolle!, roleManager);
                await ErstellenUndAktualisierenBerechtigung(adminRolle!, roleManager);
                await LöschenBerechtigung(adminRolle!, roleManager);

            }
        }
        public static async Task LesenBerechtigung(AppRolle appRolle,RoleManager<AppRolle> roleManager)
        {
            await roleManager.AddClaimAsync(appRolle!, new Claim
                    ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Vorrat.Lesen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Bestellung.Lesen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Katalog.Lesen));
        }
        public static async Task ErstellenUndAktualisierenBerechtigung(AppRolle appRolle, RoleManager<AppRolle> roleManager)
        {
            await roleManager.AddClaimAsync(appRolle!, new Claim
                    ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Vorrat.Erstellen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Bestellung.Erstellen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Katalog.Erstellen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
                    ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Vorrat.Aktualisieren));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Bestellung.Aktualisieren));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Katalog.Aktualisieren));
        }
        public static async Task LöschenBerechtigung(AppRolle appRolle, RoleManager<AppRolle> roleManager)
        {
            await roleManager.AddClaimAsync(appRolle!, new Claim
                    ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Vorrat.Löschen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Bestellung.Löschen));
            await roleManager.AddClaimAsync(appRolle!, new Claim
               ("Berechtigungen", BerechtigungenRoot.Berechtigungen.Katalog.Löschen));
        }
    }
}
