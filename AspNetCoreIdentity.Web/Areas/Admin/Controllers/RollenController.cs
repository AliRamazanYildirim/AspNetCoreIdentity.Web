using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.Areas.Admin.FluentValidierer;
using Microsoft.EntityFrameworkCore;

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
            var rollen = await _roleManager.Roles.Select(x => new AuflistungRollenAnscihtModell
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

            return RedirectToAction(nameof(RollenController.Index));
        }
    }
}
