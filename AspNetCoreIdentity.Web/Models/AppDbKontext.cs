using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Web.Models
{
    public class AppDbKontext:IdentityDbContext<AppBenutzer,AppRolle,string>
    {
        public AppDbKontext(DbContextOptions<AppDbKontext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppBenutzer>().HasData(
                    new AppBenutzer
                    {
                        Id = 1.ToString(),
                        UserName = "eliflamra",
                        Email = "eliflamrayildirim@gmail.com",
                        PhoneNumber = "015126267282",
                        Geburtsdatum = new DateTime(2024, 3, 17),
                        Geschlecht = Geschlecht.Frau,
                        Stadt = "Frankfurt"
                    },
                    new AppBenutzer
                    {
                        Id = 2.ToString(),
                        UserName = "muhammedalparslan",
                        Email = "muhammedalparslanyildirim@gmail.com",
                        PhoneNumber = "015126267217",
                        Geburtsdatum = new DateTime(2025, 3, 17),
                        Geschlecht = Geschlecht.Mann,
                        Stadt = "Frankfurt"
                    });
            base.OnModelCreating(builder);
        }
    }
}
