using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentity.Web.Erweiterungen;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly BenutzerValidator _validation;

        public HomeController(ILogger<HomeController> logger, UserManager<AppBenutzer> userManager, BenutzerValidator validation, SignInManager<AppBenutzer> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _validation = validation;
            _signInManager = signInManager;
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

        public IActionResult Einloggen()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Einloggen(EinloggenAnsichtModell anfrage, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            if (anfrage.Email != null && anfrage.Passwort != null)
            {
                var gibtsBenutzer = await _userManager.FindByEmailAsync(anfrage.Email);

                if (gibtsBenutzer == null)
                {
                    ModelState.AddModelError(string.Empty, "Email und Passwort stimmen nicht überein.");
                    return View();
                }

                var einloggenResultat = await _signInManager.PasswordSignInAsync(gibtsBenutzer, anfrage.Passwort,
                    anfrage.ErrinnereMich, true);

                if (einloggenResultat.Succeeded)
                {
                    // Null-Check für returnUrl vor Verwendung in Redirect
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    // Behandlung des Falls, dass returnUrl null ist
                    else
                    {
                        // Umleitung zu einer Standardaktion, wenn returnUrl null ist
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (einloggenResultat.IsLockedOut)
                {
                    ModelState.AddModelStateFehlerListe(new List<string>()
                    {
                        "Sie können erst nach 3 Minuten eintreten."

                    });
                    return View();

                }

                ModelState.AddModelStateFehlerListe(new List<string>()
                {
                     "Email und Passwort stimmen nicht überein.",
                     $"Anzahl der erfolglosen Einträge {await _userManager.GetAccessFailedCountAsync(gibtsBenutzer)}"
                });
            }

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

            if (identityResultat.Succeeded)
            {
                TempData["ErfolgsNachricht"] = "Der Mitgliedschaftsprozess war erfolgreich.";
                return RedirectToAction(nameof(HomeController.Anmelden));
            }

            ModelState.AddModelStateFehlerListe(identityResultat.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult PasswortVergessen()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswortVergessen(PasswortVergessenAnsichtModell anfrage)
        {
            if (anfrage == null || string.IsNullOrEmpty(anfrage.Email))
            {
                ModelState.AddModelError(String.Empty, "Ungültige Anfrage.");
                return View();
            }
            var gibtsBenutzer = await _userManager.FindByEmailAsync(anfrage.Email);

            if (gibtsBenutzer == null) 
            {
                ModelState.AddModelError(String.Empty, "Es wurde kein Benutzer mit dieser E-Mail-Adresse gefunden.");
                return View();
            }

            string passwordZurücksetzenToken =await _userManager.GeneratePasswordResetTokenAsync(gibtsBenutzer);
            var passwortZurücksetzenLink = Url.Action("PasswortZurücksetzen", "Home", new
            {
                userId = gibtsBenutzer.Id,
                Token = passwordZurücksetzenToken
            });

            TempData["ErfolgsNachricht"] = "Der Link zur Erneuerung des Passworts wurde an Ihre E-Mail-Adresse gesendet.";
            return RedirectToAction(nameof(PasswortVergessen));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}