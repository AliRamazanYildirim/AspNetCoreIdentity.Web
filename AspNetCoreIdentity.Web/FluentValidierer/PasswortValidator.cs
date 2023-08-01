using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class PasswortValidator : IPasswordValidator<AppBenutzer>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppBenutzer> manager, AppBenutzer user, string? password)
        {
            var fehler=new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                fehler.Add(new()
                {
                    Code = "PasswortEnthältBenutzername",
                    Description = "Das Passwortfeld darf nicht den Benutzernamen enthalten."
                });
            }

            if(password.ToLower().StartsWith("1234"))
            {
                fehler.Add(new()
                {
                    Code = "PasswortEnthältFortlaufendenZahlen",
                    Description = "Das Passwortfeld darf keine fortlaufenden Zahlen enthalten."
                });
            }

            if(fehler.Any())
            {
                return Task.FromResult(IdentityResult.Failed(fehler.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
