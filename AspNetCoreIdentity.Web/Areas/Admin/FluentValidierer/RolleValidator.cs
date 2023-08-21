using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Areas.Admin.Models;
using FluentValidation;

namespace AspNetCoreIdentity.Web.Areas.Admin.FluentValidierer
{
    public class RolleValidator : AbstractValidator<RolleErstellenAnsichtModell>
    {
        public string NichtLeereNachricht { get; } = "{PropertyName}sfeld darf nicht leer sein";
        public RolleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName}nsfeld darf nicht leer sein");
        }
    }
}
