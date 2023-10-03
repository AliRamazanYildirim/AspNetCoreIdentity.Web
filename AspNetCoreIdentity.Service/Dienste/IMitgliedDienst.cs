using AspNetCoreIdentity.Core.AnsichtModelle;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Service.Dienste
{
    public interface IMitgliedDienst
    {
        Task<BenutzerAnsichtModell> AufrufenBenutzerAnsichtModellNachNameAsync(string benutzerName);
        Task AusloggenAsync();
        Task<bool> ÜberprüfePasswortÄnderungAsync(string benutzerName, string passwort);
        Task<(bool, IEnumerable<IdentityError>?)> PasswortÄnderungAsync(string benutzerName, string altesPasswort, string neuesPasswort);
        Task<BenutzerBearbeitenAnsichtModell> AufrufenBenutzerBearbeitenAnsichtModellNachNameAsync(string benutzerName);
        SelectList GeschlechtSelectList();
        Task<(bool, IEnumerable<IdentityError>?)> BenutzerBearbeitenAsync(BenutzerBearbeitenAnsichtModell anfrage, string benutzerName);
        List<ClaimAnsichtModell> AufrufenClaim(ClaimsPrincipal principal);
    }
}
