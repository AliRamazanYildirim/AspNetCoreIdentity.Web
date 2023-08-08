using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class UserValidator : IUserValidator<AppBenutzer>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppBenutzer> manager, AppBenutzer user)
        {
            var fehler = new List<IdentityError>();

            if (user != null && !string.IsNullOrEmpty(user.UserName))
            {
                var istNumerisch = int.TryParse(user.UserName[0]!.ToString(), out _);

                if (istNumerisch)
                {
                    fehler.Add(new IdentityError
                    {
                        Code = "BenutzernameEnthältErsterBuchstabeZiffer",
                        Description = "Das erste Zeichen des Benutzernamens darf keinen numerischen Wert enthalten."
                    });
                }
            }
            else
            {
                fehler.Add(new IdentityError
                {
                    Code = "BenutzernameUngültig",
                    Description = "Der Benutzername darf nicht null oder leer sein."
                });
            }

            if (fehler.Any())
            {
                return Task.FromResult(IdentityResult.Failed(fehler.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }

    }
}
