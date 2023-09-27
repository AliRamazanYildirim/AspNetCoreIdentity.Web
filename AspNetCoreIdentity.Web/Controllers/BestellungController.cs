using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Web.Controllers
{
    public class BestellungController : Controller
    {
        [Authorize(Policy = "BestellungBerechtigungLesenOderLöschenPolicy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
