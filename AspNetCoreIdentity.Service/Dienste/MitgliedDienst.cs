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

        public async Task<BenutzerAnsichtModell> RufeBenutzerAnsichtModellNachNameAufAsync(string benutzerName)
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

        public async Task<bool> ÜberprüfePasswortÄnderungAsync(string benutzerName, string passwort)
        {
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(benutzerName))!;

            return await _userManager.CheckPasswordAsync(aktuellerBenutzer, passwort);
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> PasswortÄnderungAsync(string benutzerName, string altesPasswort, string neuesPasswort)
        {
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(benutzerName))!;

            var resultat = await _userManager.ChangePasswordAsync(aktuellerBenutzer, altesPasswort, neuesPasswort);
            if(!resultat.Succeeded)
            {
                return (false, resultat.Errors);
            }
            await _userManager.UpdateSecurityStampAsync(aktuellerBenutzer);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(aktuellerBenutzer, neuesPasswort, true, false);

            return (true, null);

        }
    }
}
