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

        public MitgliedController(SignInManager<AppBenutzer> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task Ausloggen()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
