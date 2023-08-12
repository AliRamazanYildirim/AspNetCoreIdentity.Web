using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    [Authorize]
    public class MitgliedController : Controller
    {
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly UserManager<AppBenutzer> _userManager;

        public MitgliedController(SignInManager<AppBenutzer> signInManager, UserManager<AppBenutzer> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
    }
}
