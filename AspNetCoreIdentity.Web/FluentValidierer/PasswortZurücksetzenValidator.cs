using AspNetCoreIdentity.Web.AnsichtModelle;
using FluentValidation;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class PasswortZurücksetzenValidator : AbstractValidator<PasswortZurücksetzenAnsichtModell>
    {
        public PasswortZurücksetzenValidator()
        {
            RuleFor(p => p.Passwort).NotEmpty().WithMessage("Ihr neues Passwort darf nicht leer sein.")
               .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
               .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(p => p.Passwort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(p => p.Passwort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(p => p.Passwort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(p => p.Passwort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");
            RuleFor(p => p.Passwort).Equal(pb => pb.PasswortBestätigen).WithMessage("Ihr Passwort stimmen nicht über.");

        }
    }
}
