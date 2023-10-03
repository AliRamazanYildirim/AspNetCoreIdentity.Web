using AspNetCoreIdentity.Core.AnsichtModelle;
using AspNetCoreIdentity.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Service.Dienste
{
    public class MitgliedDienst: IMitgliedDienst
    {
        private readonly UserManager<AppBenutzer> _userManager;
        private readonly SignInManager<AppBenutzer> _signInManager;
        private readonly IFileProvider _fileProvider;
        private readonly IHttpContextAccessor _accessor;

        public MitgliedDienst(UserManager<AppBenutzer> userManager, SignInManager<AppBenutzer> signInManager, IFileProvider fileProvider, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
            _accessor = accessor;
        }

        public async Task AusloggenAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<BenutzerAnsichtModell> AufrufenBenutzerAnsichtModellNachNameAsync(string benutzerName)
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

        public async Task<BenutzerBearbeitenAnsichtModell> AufrufenBenutzerBearbeitenAnsichtModellNachNameAsync(string benutzerName)
        {
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(benutzerName))!;

            return new BenutzerBearbeitenAnsichtModell()
            {
                BenutzerName = aktuellerBenutzer!.UserName,
                Email = aktuellerBenutzer.Email,
                Telefonnummer = aktuellerBenutzer.PhoneNumber,
                Stadt = aktuellerBenutzer.Stadt,
                Geburtsdatum = aktuellerBenutzer.Geburtsdatum,
                Geschlecht = aktuellerBenutzer.Geschlecht
            };
        }
        SelectList IMitgliedDienst.GeschlechtSelectList() => new (Enum.GetNames(typeof(Geschlecht)));
        public async Task<(bool, IEnumerable<IdentityError>?)> BenutzerBearbeitenAsync(BenutzerBearbeitenAnsichtModell anfrage, string benutzerName)
        {
            var aktuellerBenutzer = (await _userManager.FindByNameAsync(benutzerName))!;

            aktuellerBenutzer.UserName = anfrage.BenutzerName;
            aktuellerBenutzer.Email = anfrage.Email;
            aktuellerBenutzer.PhoneNumber = anfrage.Telefonnummer;
            aktuellerBenutzer.Geburtsdatum = anfrage.Geburtsdatum;
            aktuellerBenutzer.Stadt = anfrage.Stadt;
            aktuellerBenutzer.Geschlecht = anfrage.Geschlecht;

            if (anfrage.Bild != null && anfrage.Bild.Length > 0)
            {
                var bildWeg = _fileProvider.GetDirectoryContents("wwwroot");
                var zufälligerDateiName = $"{Guid.NewGuid()}{Path.GetExtension(anfrage.Bild.FileName)}";
                var neuerBildWeg = Path.Combine(bildWeg!.First(x => x.Name == "benutzerbilder").PhysicalPath!, zufälligerDateiName);
                using var strom = new FileStream(neuerBildWeg, FileMode.Create);
                await anfrage.Bild.CopyToAsync(strom);
                aktuellerBenutzer.Bild = zufälligerDateiName;

            }

            var benutzerAktualisieren = await _userManager.UpdateAsync(aktuellerBenutzer);
            if (!benutzerAktualisieren.Succeeded)
            {
                return (false, benutzerAktualisieren.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(aktuellerBenutzer);
            await _signInManager.SignOutAsync();

            if (anfrage.Geburtsdatum.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(aktuellerBenutzer, true, new[]
                {
                        new Claim("Geburtsdatum",aktuellerBenutzer.Geburtsdatum!.Value.ToString())
                });
            }
            else
                await _signInManager.SignInAsync(aktuellerBenutzer, true);
            return (true, null);
        }

        public List<ClaimAnsichtModell> AufrufenClaim(ClaimsPrincipal principal)
        {
            return  _accessor.HttpContext!.User.Claims.Select(x => new ClaimAnsichtModell
            {
                Anbieter = x.Issuer,
                Typ = x.Type,
                Wert = x.Value
            }).ToList();
        }
    }
}
