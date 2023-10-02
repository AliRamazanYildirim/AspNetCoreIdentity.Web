using AspNetCoreIdentity.Core.AnsichtModelle;
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
    }
}
