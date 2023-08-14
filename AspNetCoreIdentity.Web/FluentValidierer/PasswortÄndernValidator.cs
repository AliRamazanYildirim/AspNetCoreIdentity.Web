using FluentValidation;
using AspNetCoreIdentity.Web.AnsichtModelle;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class PasswortÄndernValidator : AbstractValidator<PasswortÄndernAnsichtsModell>
    {
        public PasswortÄndernValidator()
        {
            RuleFor(p => p.AltesPasswort).NotEmpty().WithMessage("Ihr altes Passwort darf nicht leer sein.")
           .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
           .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(b => b.AltesPasswort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(b => b.AltesPasswort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(b => b.AltesPasswort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(b => b.AltesPasswort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");

            RuleFor(p => p.NeuesPasswort).NotEmpty().WithMessage("Ihr neues Passwort darf nicht leer sein.")
           .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
           .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(b => b.NeuesPasswort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(b => b.NeuesPasswort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(b => b.NeuesPasswort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(b => b.NeuesPasswort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");
            RuleFor(x => x.PasswortNeuBestätigen).NotEmpty().WithMessage("Bitte bestätigen Sie Ihr Passwort.");
        }
    }
}
