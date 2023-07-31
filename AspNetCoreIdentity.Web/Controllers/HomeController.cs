using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly BenutzerValidator _validation;

        public HomeController(ILogger<HomeController> logger, UserManager<AppBenutzer> userManager, BenutzerValidator validation)
        {
            _logger = logger;
            _userManager = userManager;
            _validation = validation;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Anmelden()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Anmelden(AnmeldenAnsichtModell anfrage)
        {          
            var validationResultat = _validation.Validate(anfrage);

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

            var identityResultat = await _userManager.CreateAsync(new()
            {
                UserName = anfrage.BenutzerName,
                PhoneNumber = anfrage.Telefonnummer,
                Email = anfrage.Email
            }, anfrage.PasswortBestätigen ?? "");           

            if(identityResultat.Succeeded)
            {
                TempData["ErfolgsNachricht"] = "Der Mitgliedschaftsprozess war erfolgreich.";
                return RedirectToAction(nameof(HomeController.Anmelden));
            }

            foreach (var identityFehler in identityResultat.Errors)
            {
                ModelState.AddModelError(string.Empty, identityFehler.Description);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}