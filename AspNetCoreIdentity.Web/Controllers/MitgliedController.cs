using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MitgliedController : Controller
    {
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly PasswortÄndernValidator _validation;

        public MitgliedController(SignInManager<AppBenutzer> signInManager, UserManager<AppBenutzer> userManager, PasswortÄndernValidator validation)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _validation = validation;
        }

        public async Task<IActionResult> Index()
        {
            var aktuellerBenutzer = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var benutzerAnsichtModell = new BenutzerAnsichtModell
            {
                BenutzerName = aktuellerBenutzer!.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber
            };
            return View(benutzerAnsichtModell);
        }

        public async Task Ausloggen()
        {
            await _signInManager.SignOutAsync();
        }
        public IActionResult PasswortÄnderung()
        {            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswortÄnderung(PasswortÄndernAnsichtsModell anfrage)
        {
            var validationResultat =await _validation.ValidateAsync(anfrage);

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
            var aktuellerBenutzer = await _userManager.FindByNameAsync(User.Identity!.Name!);
            if (aktuellerBenutzer == null)
            {
                ModelState.AddModelError(string.Empty, "Dieser Benutzer wurde nicht gefunden.");
                return View();
            }

            var altesPasswortPrüfen =await _userManager.CheckPasswordAsync(aktuellerBenutzer, anfrage.AltesPasswort);
            if (!altesPasswortPrüfen)
            {
                ModelState.AddModelError(string.Empty, "Ihr altes Passwort ist falsch.");
                return View();
            }

            var resultat = await _userManager.ChangePasswordAsync(aktuellerBenutzer, anfrage.AltesPasswort, anfrage.NeuesPasswort);
            if (!resultat.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(resultat.Errors.Select(x => x.Description).ToList());
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(aktuellerBenutzer);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(aktuellerBenutzer, anfrage.NeuesPasswort, true, false);

            TempData["ErfolgsNachricht"] = "Ihr Passwort wurde erfolgreich geändert.";
            return View();
        }
        public async Task<IActionResult> BenutzerBearbeiten()
        {
            ViewBag.geschlecht = new SelectList(Enum.GetNames(typeof(Geschlecht)));

            var aktuellerBenutzer = await _userManager.FindByNameAsync(User.Identity!.Name!)!;

            var benutzerAnscihtModell = new BenutzerBearbeitenAnsichtModell()
            {
                BenutzerName = aktuellerBenutzer!.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber,
                Stadt = aktuellerBenutzer.Stadt,
                Geburtsdatum = aktuellerBenutzer.Geburtsdatum,
                Geschlecht = aktuellerBenutzer.Geschlecht,
            };
            return View(benutzerAnscihtModell);
        }
    }
}
