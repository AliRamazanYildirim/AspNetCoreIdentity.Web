using AspNetCoreIdentity.Core.AnsichtModelle;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Service.Dienste
{
    public interface IMitgliedDienst
    {
        Task<BenutzerAnsichtModell> RufeBenutzerAnsichtModellNachNameAufAsync(string benutzerName);
        Task AusloggenAsync();
        Task<bool> ÜberprüfePasswortÄnderungAsync(string benutzerName, string passwort);
        Task<(bool, IEnumerable<IdentityError>?)> PasswortÄnderungAsync(string benutzerName, string altesPasswort, string neuesPasswort);

    }
}
