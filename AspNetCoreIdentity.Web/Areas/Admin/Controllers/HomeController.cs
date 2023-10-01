using AspNetCoreIdentity.Core.Models;
using AspNetCoreIdentity.Web.Areas.Admin.Models;
using AspNetCoreIdentity.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppBenutzer> _userManager;

        public HomeController(UserManager<AppBenutzer> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BenutzerListe()
        {
            var benutzerListe = await _userManager.Users.ToListAsync();
            var benutzerAnsichtModellListe = benutzerListe.Select(x => new BenutzerAnsichtModell()
            {
                BenutzerID = x.Id,
                BenutzerName = x.UserName,
                BenutzerEmail = x.Email
            }).ToList();
            return View(benutzerAnsichtModellListe);
        }
    }
}
