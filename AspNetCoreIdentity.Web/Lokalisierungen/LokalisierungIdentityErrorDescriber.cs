using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Lokalisierungen
{
    public class LokalisierungIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplizierterBenutzernamen",
                Description = $"Dieser Benutzername {userName} von einem anderen Benutzer übernommen wurde."
            };
            //return base.DuplicateUserName(userName);
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplizierterEmail",
                Description = $"Diese Email {email} von einem anderen Benutzer übernommen wurde."
            };
            //return base.DuplicateEmail(email);
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswortZuKurz",
                Description = "Ihr Passwort ist zu kurz."
            };
            //return base.PasswordTooShort(length);
        }
    }
}
