using AspNetCoreIdentity.Core.AnsichtModelle;
using AspNetCoreIdentity.Core.FluentValidierer;
using AspNetCoreIdentity.Core.Models;
using AspNetCoreIdentity.Service.Dienste;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.FluentValidierer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

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
        private string BenutzerName => User.Identity!.Name!;
        private readonly IMitgliedDienst _mitgliedDienst;

        public MitgliedController(SignInManager<AppBenutzer> signInManager, UserManager<AppBenutzer> userManager,
            PasswortÄndernValidator validation, BenutzerBearbeitenValidator validator, IFileProvider fileProvider,
            IHttpContextAccessor accessor, IMitgliedDienst mitgliedDienst)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _validation = validation;
            _validator = validator;
            _fileProvider = fileProvider;
            _accessor = accessor;
            _mitgliedDienst = mitgliedDienst;
        }

        public async Task<IActionResult> Index()
        {
            _ = _accessor.HttpContext!.User.Claims.ToList();
            
            return View(await _mitgliedDienst.AufrufenBenutzerAnsichtModellNachNameAsync(BenutzerName));
        }

        public async Task Ausloggen()
        {
           await _mitgliedDienst.AusloggenAsync();
        }
        public IActionResult PasswortÄnderung()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswortÄnderung(PasswortÄndernAnsichtsModell anfrage)
        {
            var validationResultat = await _validation.ValidateAsync(anfrage);

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

            if (!await _mitgliedDienst.ÜberprüfePasswortÄnderungAsync(BenutzerName, anfrage.AltesPasswort))
            {
                ModelState.AddModelError(string.Empty, "Ihr altes Passwort ist falsch.");
                return View();
            }

            var (istErfolgreich, fehler) = await _mitgliedDienst.PasswortÄnderungAsync(BenutzerName, anfrage.AltesPasswort, anfrage.NeuesPasswort);
            if (!istErfolgreich)
            {
                ModelState.AddModelStateFehlerListe(fehler!);
                return View();
            }
           

            TempData["ErfolgsNachricht"] = "Ihr Passwort wurde erfolgreich geändert.";
            return View();
        }

        public async Task<IActionResult> BenutzerBearbeiten()
        {
            ViewBag.geschlecht = _mitgliedDienst.GeschlechtSelectList(); 
            return View(await _mitgliedDienst.AufrufenBenutzerBearbeitenAnsichtModellNachNameAsync(BenutzerName));
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

            var (istErfolgreich, fehler) = await _mitgliedDienst.BenutzerBearbeitenAsync(anfrage, BenutzerName);

            if (!istErfolgreich)
            {
                ModelState.AddModelStateFehlerListe(fehler!);
                return View();
            }

            TempData["ErfolgsNachricht"] = "Die Mitgliederinformationen wurden erfolgreich geändert.";

            return View(await _mitgliedDienst.AufrufenBenutzerBearbeitenAnsichtModellNachNameAsync(BenutzerName));
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
            
            return View(_mitgliedDienst.AufrufenClaim(User));
        }

        [Authorize(Policy = "AdminStadtPolicy")]
        [HttpGet]
        public IActionResult AdminStadtPolicySeite()
        {
            return View();
        }

        [Authorize(Policy = "UmtauschPolicy")]
        [HttpGet]
        public IActionResult UmtauschPolicySeite()
        {
            return View();
        }

        [Authorize(Policy = "GewaltPolicy")]
        [HttpGet]
        public IActionResult GewaltPolicySeite()
        {
            return View();
        }
    }
}
