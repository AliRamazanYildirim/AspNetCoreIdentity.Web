using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentity.Web.Erweiterungen;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RollenController : Controller
    {
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly RoleManager<AppRolle> _roleManager;

        public RollenController(UserManager<AppBenutzer> userManager, RoleManager<AppRolle> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RolleErstellen()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RolleErstellen(RolleErstellenAnsichtModell anfrage)
        {
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
