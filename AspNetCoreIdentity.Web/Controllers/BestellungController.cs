using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class BestellungController : Controller
    {
        [Authorize(Policy = "BerechtigungenRoot.Berechtigungen.Bestellung.Lesen")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
