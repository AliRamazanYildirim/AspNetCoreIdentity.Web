using AspNetCoreIdentity.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Core.Models
{
    public class AppBenutzer:IdentityUser
    {
        public string? Stadt { get; set; }
        public string? Bild { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public Geschlecht? Geschlecht { get; set; }
    }
}
