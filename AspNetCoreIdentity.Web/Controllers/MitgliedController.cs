using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class MitgliedController : Controller
    {
        private readonly SignInManager<AppBenutzer> _signInManager;

        public MitgliedController(SignInManager<AppBenutzer> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Ausloggen()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
