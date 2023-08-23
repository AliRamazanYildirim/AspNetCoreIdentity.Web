using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Areas.Admin.Models;
using FluentValidation;

namespace AspNetCoreIdentity.Web.Areas.Admin.FluentValidierer
{
    public class RolleAktualisierenValidator : AbstractValidator<RolleAktualisierenAnscihtModell>
    {
        public string NichtLeereNachricht { get; } = "{PropertyName}sfeld darf nicht leer sein";
        public RolleAktualisierenValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName}nsfeld darf nicht leer sein");
        }
    }
}
