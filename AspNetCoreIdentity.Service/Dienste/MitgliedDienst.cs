using AspNetCoreIdentity.Core.AnsichtModelle;
using AspNetCoreIdentity.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Service.Dienste
{
    public class MitgliedDienst: IMitgliedDienst
    {
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly SignInManager<AppBenutzer> _signInManager;

        public MitgliedDienst(UserManager<AppBenutzer> userManager, SignInManager<AppBenutzer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task AusloggenAsync()
        {
            await _signInManager.SignOutAsync();
        }

        async Task<BenutzerAnsichtModell> IMitgliedDienst.RufeBenutzerAnsichtModellNachNameAufAsync(string benutzerName)
        {
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(benutzerName))!;

            return new BenutzerAnsichtModell
            {
                BenutzerName = aktuellerBenutzer.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber,
                BildUrl = aktuellerBenutzer.Bild
            };
        }
    }
}
