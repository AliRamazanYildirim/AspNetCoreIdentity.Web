using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MitgliedController : Controller
    {
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly PasswortÄndernValidator _validation;
        private readonly BenutzerBearbeitenValidator _validator;
        private readonly IFileProvider _fileProvider;
        private readonly IHttpContextAccessor _accessor;

        public MitgliedController(SignInManager<AppBenutzer> signInManager, UserManager<AppBenutzer> userManager,
            PasswortÄndernValidator validation, BenutzerBearbeitenValidator validator, IFileProvider fileProvider,
            IHttpContextAccessor accessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _validation = validation;
            _validator = validator;
            _fileProvider = fileProvider;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            _ = _accessor.HttpContext!.User.Claims.ToList();
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var benutzerAnsichtModell = new BenutzerAnsichtModell
            {
                BenutzerName = aktuellerBenutzer.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber,
                BildUrl = aktuellerBenutzer.Bild
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
                Geschlecht = aktuellerBenutzer.Geschlecht
            };
            return View(benutzerAnscihtModell);
        }

        [HttpPost]
        public async Task<IActionResult> BenutzerBearbeiten(BenutzerBearbeitenAnsichtModell anfrage)
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

            var aktuellerBenutzer = await _userManager.FindByNameAsync(User.Identity!.Name!);

            aktuellerBenutzer!.UserName = anfrage.BenutzerName;
            aktuellerBenutzer.Email = anfrage.Email;
            aktuellerBenutzer.PhoneNumber = anfrage.Telefonnummer;
            aktuellerBenutzer.Geburtsdatum = anfrage.Geburtsdatum;
            aktuellerBenutzer.Stadt = anfrage.Stadt;
            aktuellerBenutzer.Geschlecht = anfrage.Geschlecht;

            if (anfrage.Bild != null && anfrage.Bild.Length > 0) 
            {
                var bildWeg = _fileProvider.GetDirectoryContents("wwwroot");
                var zufälligerDateiName = $"{Guid.NewGuid()}{Path.GetExtension(anfrage.Bild.FileName)}";
                var neuerBildWeg = Path.Combine(bildWeg!.First(x => x.Name == "benutzerbilder").PhysicalPath!, zufälligerDateiName);
                using var strom = new FileStream(neuerBildWeg, FileMode.Create);
                await anfrage.Bild.CopyToAsync(strom);
                aktuellerBenutzer.Bild = zufälligerDateiName;

            }
            var benutzerAktualisieren = await _userManager.UpdateAsync(aktuellerBenutzer);

            if (!benutzerAktualisieren.Succeeded)
            {
                ModelState.AddModelStateFehlerListe(benutzerAktualisieren.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(aktuellerBenutzer);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(aktuellerBenutzer, true);

            TempData["ErfolgsNachricht"] = "Die Mitgliederinformationen wurden erfolgreich geändert.";

            var benutzerAnscihtModell = new BenutzerBearbeitenAnsichtModell()
            {
                BenutzerName = aktuellerBenutzer!.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber,
                Stadt = aktuellerBenutzer.Stadt,
                Geburtsdatum = aktuellerBenutzer.Geburtsdatum,
                Geschlecht = aktuellerBenutzer.Geschlecht
            };

            return View(benutzerAnscihtModell);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                throw new ArgumentException($"\"{nameof(ReturnUrl)}\" kann nicht NULL oder leer sein.", nameof(ReturnUrl));
            }

            string nachricht = @"Sie sind nicht berechtigt, diese Seite anzusehen. 
                        Bitte wenden Sie sich an den Seitenadministrator, um eine Genehmigung zu erhalten.";
            ViewBag.nachricht = nachricht;
            return View();
        }

        [HttpGet]
        public IActionResult Claims()
        {
            var benutzerClaimsListe = _accessor.HttpContext!.User.Claims.Select(x => new ClaimAnsichtModell
            {
                Anbieter = x.Issuer,
                Typ = x.Type,
                Wert = x.Value
            }).ToList();
            return View(benutzerClaimsListe);
        }

        [Authorize(Policy = "AdminStadtPolicy")]
        [HttpGet]
        public IActionResult AdminStadtPolicySeite()
        {
            return View();
        }
    }
}
