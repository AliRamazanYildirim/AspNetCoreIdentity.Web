using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.Areas.Admin.FluentValidierer;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentity.Web.Controllers;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RollenController : Controller
    {
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly RoleManager<AppRolle> _roleManager;
        private readonly RolleValidator _validator;

        public RollenController(UserManager<AppBenutzer> userManager, RoleManager<AppRolle> roleManager, RolleValidator validator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            var rollen = await _roleManager.Roles.Select(x => new RollenAuflistenAnscihtModell
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(rollen);
        }
        public IActionResult RolleErstellen()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RolleErstellen(RolleErstellenAnsichtModell anfrage)
        {
            var validationResultat = await _validator.ValidateAsync(anfrage);
            if (!validationResultat.IsValid)
            {
                foreach (var error in validationResultat.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                // Zeigen die Seite erneut an, wenn Validierungsfehler vorliegen.
                return View(anfrage);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (anfrage is null)
            {
                throw new ArgumentNullException(nameof(anfrage));
            }

            var resultat = await _roleManager.CreateAsync(new AppRolle
            {
                Name = anfrage.Name
            });

            if (!resultat.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(resultat.Errors);
                return View();
            }

            TempData["ErfolgsNachricht"] = "Die Rolleninformationen wurden erstellt.";
            return RedirectToAction(nameof(MitgliedController.Index));
        }

        public async Task<IActionResult> RolleAktualisieren(string id)
        {
            var rolleAktualisieren = await _roleManager.FindByIdAsync(id);
            return rolleAktualisieren == null
                ? throw new Exception("Keine Rolle zu aktualisieren")
                : (IActionResult)View(new RolleAktualisierenAnscihtModell()
                {
                    Id = rolleAktualisieren.Id,
                    Name = rolleAktualisieren.Name
                });
        }
        [HttpPost]
        public async Task<IActionResult> RolleAktualisieren(RolleAktualisierenAnscihtModell anfrage)
        {
            var rolleAktualisieren = await _roleManager.FindByIdAsync(anfrage.Id!) ?? throw new Exception("Keine Rolle zu aktualisieren");
            rolleAktualisieren.Name = anfrage.Name;
            await _roleManager.UpdateAsync(rolleAktualisieren);
            ViewData["ErfolgsNachricht"] = "Die Rolleninformationen wurden aktualisiert.";
            return View();
               
        }
        public async Task<IActionResult> RolleLöschen(string id)
        {
            var rolleLöschen = await _roleManager.FindByIdAsync(id) ?? throw new Exception("Keine Rolle zu löshen");
            var resultat = await _roleManager.DeleteAsync(rolleLöschen);

            if(!resultat.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Keine Rolle wurde gefunden");
            }
            TempData["ErfolgsNachricht"] = "Die Rolleninformationen wurden gelöscht.";
            return RedirectToAction(nameof(MitgliedController.Index));
        }
        public async Task<IActionResult> RolleZuweisen(string id)
        {
            var aktuellerBenutzer = await _userManager.FindByIdAsync(id);
            ViewBag.benutzerID = id;
            var rollen = await _roleManager.Roles.ToListAsync();
            var rollenAnscihtModellList = new List<RollenZuweisenAnscihtModell>();
            var benutzerRollen = await _userManager.GetRolesAsync(aktuellerBenutzer!);

            foreach(var rolle in rollen)
            {
                var rollenZuweisenAnscihtModell = new RollenZuweisenAnscihtModell()
                {
                    Id = rolle.Id,
                    Name = rolle.Name
                };

                if(benutzerRollen.Contains(rolle.Name!))
                {
                    rollenZuweisenAnscihtModell.Existiert = true;
                }
                rollenAnscihtModellList.Add(rollenZuweisenAnscihtModell);
            }
            return View(rollenAnscihtModellList);
        }
        [HttpPost]
        public async Task<IActionResult> RolleZuweisen(string benutzerID, List<RollenZuweisenAnscihtModell> anfrage)
        {
            var benutzerFürRolleZuweisen = await _userManager.FindByIdAsync(benutzerID)!;
            foreach(var rolle in anfrage)
            {
                if (rolle.Existiert)
                {
                    await _userManager.AddToRoleAsync(benutzerFürRolleZuweisen!, rolle.Name!);
                }
                else
                    await _userManager.RemoveFromRoleAsync(benutzerFürRolleZuweisen!, rolle.Name!);
            }
            return RedirectToAction(nameof(HomeController.BenutzerListe), "Home");
        }
    }
}
