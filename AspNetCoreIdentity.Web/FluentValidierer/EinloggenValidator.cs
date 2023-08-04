using AspNetCoreIdentity.Web.AnsichtModelle;
using FluentValidation;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class EinloggenValidator : AbstractValidator<EinloggenAnsichtModell>
    {
        public string NichtLeereNachricht { get; } = "{PropertyName}sfeld darf nicht leer sein";
        public EinloggenValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(NichtLeereNachricht)
           .EmailAddress().WithMessage("Die E-Mail hat nicht das richtige Format");
            RuleFor(p => p.Passwort).NotEmpty().WithMessage("Ihr Passwort darf nicht leer sein.")
                  .MinimumLength(8).WithMessage("Ihre Passwortlänge muss mindestens 8 betragen.")
                  .MaximumLength(16).WithMessage("Ihre Passwortlänge darf 16 nicht überschreiten.");
            RuleFor(b => b.Passwort).Matches(@"[A-Z]+").WithMessage("Ihr Passwort muss mindestens einen Großbuchstaben enthalten.");
            RuleFor(b => b.Passwort).Matches(@"[a-z]+").WithMessage("Ihr Passwort muss mindestens einen Kleinbuchstaben enthalten.");
            RuleFor(b => b.Passwort).Matches(@"[0-9]+").WithMessage("Ihr Passwort muss mindestens eine Nummer enthalten.");
            RuleFor(b => b.Passwort).Matches(@"[\!\?\*\.]*$").WithMessage("Ihr Passwort muss mindestens ein (!? *.) enthalten.");
        }
    }
}
