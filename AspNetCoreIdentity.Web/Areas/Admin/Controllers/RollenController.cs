using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RollenController : Controller
    {
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly RoleManager<AppRolle> _roleManager;

        public RollenController(UserManager<AppBenutzer> userManager, RoleManager<AppRolle> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
