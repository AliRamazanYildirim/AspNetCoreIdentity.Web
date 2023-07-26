using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Models
{
    public class AppDbKontext:IdentityDbContext<AppBenutzer,AppRolle,string>
    {
        public AppDbKontext(DbContextOptions<AppDbKontext> options) : base(options)
        {

        }
    }
}
