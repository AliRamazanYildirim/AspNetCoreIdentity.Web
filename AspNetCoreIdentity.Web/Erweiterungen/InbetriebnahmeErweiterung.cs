using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Lokalisierungen;
using AspNetCoreIdentity.Web.Models;
using FluentValidation;

namespace AspNetCoreIdentity.Web.Erweiterungen
{
    public static class InbetriebnahmeErweiterung
    {
        public static void AddIdentityMitErweiterung(this IServiceCollection services)
        {
            services.AddIdentity<AppBenutzer, AppRolle>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZçşğüö@!#$%_-1234567890";

                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            }).AddPasswordValidator<PasswortValidator>().AddUserValidator<UserValidator>()
            .AddErrorDescriber<LokalisierungIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbKontext>();

            services.AddScoped<IValidator<AnmeldenAnsichtModell>, BenutzerValidator>();
            services.AddScoped<IValidator<EinloggenAnsichtModell>, EinloggenValidator>();
            services.AddValidatorsFromAssemblyContaining<BenutzerValidator>();
        }

    }
}
