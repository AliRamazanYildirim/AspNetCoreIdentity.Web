using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Models
{
    public class AppBenutzer:IdentityUser
    {
        public string? Stadt { get; set; }
        public string? Bild { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public Geschlecht? Geschlecht { get; set; }
    }
}
