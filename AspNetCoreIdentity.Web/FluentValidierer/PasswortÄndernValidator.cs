using FluentValidation;
using AspNetCoreIdentity.Web.AnsichtModelle;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class PasswortÄndernValidator : AbstractValidator<PasswortÄndernAnsichtsModell>
    {
        public PasswortÄndernValidator()
        {
            RuleFor(ap => ap.AltesPasswort).NotEmpty().WithMessage("Ihr altes Passwort darf nicht leer sein.")
           .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
           .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(ap => ap.AltesPasswort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(ap => ap.AltesPasswort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(ap => ap.AltesPasswort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(ap => ap.AltesPasswort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");

            RuleFor(np => np.NeuesPasswort).NotEmpty().WithMessage("Ihr neues Passwort darf nicht leer sein.")
           .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
           .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(np => np.NeuesPasswort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(np => np.NeuesPasswort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(np => np.NeuesPasswort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(np => np.NeuesPasswort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");
            RuleFor(np => np.NeuesPasswort).Equal(pb => pb.PasswortNeuBestätigen).WithMessage("Ihr Passwort stimmen nicht über.");
        }
    }
}
