using AspNetCoreIdentity.Core.AnsichtModelle;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.Dienste;
using AspNetCoreIdentity.Core.OptionModell;
using NuGet.Common;
using System.Security.Claims;
using AspNetCoreIdentity.Core.Models;
using AspNetCoreIdentity.Core.FluentValidierer;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly BenutzerValidator _validation;
        private readonly EinloggenValidator _validator;
        private readonly IEmailDienst _emailDienst;

        public HomeController(ILogger<HomeController> logger, UserManager<AppBenutzer> userManager, BenutzerValidator validation, SignInManager<AppBenutzer> signInManager, IEmailDienst emailDienst, EinloggenValidator validator)
        {
            _logger = logger;
            _userManager = userManager;
            _validation = validation;
            _signInManager = signInManager;
            _emailDienst = emailDienst;
            _validator = validator;
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

            if (!identityResultat.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(identityResultat.Errors.Select(x => x.Description).ToList());
                return View();
            }
            var umtauschClaim = new Claim("AblaufDatumDesUmtauschs", DateTime.Now.AddDays(1).ToString());
            var aktuallerBenutzer = await _userManager.FindByNameAsync(anfrage.BenutzerName!);

            var claimResultat = await _userManager.AddClaimAsync(aktuallerBenutzer!, umtauschClaim);
            if (!claimResultat.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(claimResultat.Errors.Select(x => x.Description).ToList());
                return View();
            }
            TempData["ErfolgsNachricht"] = "Der Mitgliedschaftsprozess war erfolgreich.";
            return RedirectToAction(nameof(HomeController.Anmelden));


        }

        public IActionResult Einloggen()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Einloggen(EinloggenAnsichtModell anfrage, string? returnUrl = null)
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

            returnUrl ??= Url.Action("Index", "Home");

            var gibtsBenutzer = await _userManager.FindByEmailAsync(anfrage.Email!);

            if (gibtsBenutzer == null)
            {
                ModelState.AddModelError(string.Empty, "Email und Passwort stimmen nicht überein.");
                return View();
            }

            var einloggenResultat = await _signInManager.PasswordSignInAsync(gibtsBenutzer, anfrage.Passwort!,
                anfrage.ErrinnereMich, true);

            if (einloggenResultat.IsLockedOut)
            {
                ModelState.AddModelStateFehlerListe(new List<string>()
                    {
                        "Sie können erst nach 3 Minuten eintreten."

                    });
                return View();

            }

            if (!einloggenResultat.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(new List<string>()
                    {
                        "Email und Passwort stimmen nicht überein.",
                        $"Anzahl der erfolglosen Einträge {await _userManager.GetAccessFailedCountAsync(gibtsBenutzer)}"
                    });
                return View();
            }

            if (gibtsBenutzer.Geburtsdatum.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(gibtsBenutzer, anfrage.ErrinnereMich, new[]
                {
                        new Claim("Geburtsdatum",gibtsBenutzer.Geburtsdatum.Value.ToString())
                });
            }
            return Redirect(returnUrl!);
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

            string passwordZurücksetzenToken = await _userManager.GeneratePasswordResetTokenAsync(gibtsBenutzer);
            var passwortZurücksetzenLink = Url.Action("PasswortZurücksetzen", "Home", new
            {
                userId = gibtsBenutzer.Id,
                Token = passwordZurücksetzenToken,
            }, HttpContext.Request.Scheme);

            await _emailDienst.SendeZurücksetzenPasswortEmail(passwortZurücksetzenLink, gibtsBenutzer.Email);

            TempData["ErfolgsNachricht"] = "Der Link zur Erneuerung des Passworts wurde an Ihre E-Mail-Adresse gesendet.";
            return RedirectToAction(nameof(PasswortVergessen));
        }

        public IActionResult PasswortZurücksetzen(string? userId, string? token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswortZurücksetzen(PasswortZurücksetzenAnsichtModell anfrage)
        {
            var userId = TempData["userId"]?.ToString();
            var token = TempData["token"]?.ToString();
            if (userId == null)
            {
                ModelState.AddModelError(String.Empty, "Es wurde kein BenutzerId gefunden");
                return View();
            }


            var gibtsBenutzer = await _userManager.FindByIdAsync(userId);

            if (gibtsBenutzer == null || token == null || string.IsNullOrEmpty(anfrage.Passwort))
            {
                ModelState.AddModelError(String.Empty, "Es wurde kein Benutzer gefunden.");
                return View();
            }

            var resultat = await _userManager.ResetPasswordAsync(gibtsBenutzer, token, anfrage.Passwort);
            if (resultat.Succeeded)
            {
                TempData["ErfolgsNachricht"] = "Ihr Passwort wurde erfolgreich erneuert.";
            }
            ModelState.AddModelStateFehlerListe(resultat.Errors.Select(x => x.Description).ToList());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}