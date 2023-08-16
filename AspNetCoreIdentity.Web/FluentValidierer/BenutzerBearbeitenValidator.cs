using AspNetCoreIdentity.Web.AnsichtModelle;
using FluentValidation;

namespace AspNetCoreIdentity.Web.FluentValidierer
{
    public class BenutzerBearbeitenValidator : AbstractValidator<BenutzerBearbeitenAnsichtModell>
    {
        public string NichtLeereNachricht { get; } = "{PropertyName}sfeld darf nicht leer sein";
        public BenutzerBearbeitenValidator()
        {
            RuleFor(x => x.BenutzerName).NotEmpty().WithMessage("{PropertyName}nsfeld darf nicht leer sein");
            RuleFor(x => x.Email).NotEmpty().WithMessage(NichtLeereNachricht)
                .EmailAddress().WithMessage("Die E-Mail hat nicht das richtige Format");
            RuleFor(x => x.Telefonnummer).NotEmpty().WithMessage(NichtLeereNachricht).
                MaximumLength(12).WithMessage("{PropertyName}sfeld kann maximal {MaxLength} sein");
            RuleFor(x => x.Geburtsdatum).NotEmpty().WithMessage("{PropertyName}sfeld darf nicht leer sein");
            RuleFor(x => x.Stadt).NotEmpty().WithMessage("{PropertyName}sfeld darf nicht leer sein");
            RuleFor(x => x.Bild).NotEmpty().WithMessage("{PropertyName}sfeld darf nicht leer sein");
            RuleFor(x => x.Geschlecht).NotEmpty().WithMessage("{PropertyName}nsfeld darf nicht leer sein");
        }
    }
}
