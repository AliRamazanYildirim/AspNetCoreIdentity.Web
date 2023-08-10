using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Dienste;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Lokalisierungen;
using AspNetCoreIdentity.Web.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Web.Erweiterungen
{
    public static class InbetriebnahmeErweiterung
    {
        public static void AddIdentityMitErweiterung(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(option =>
            {
                option.TokenLifespan = TimeSpan.FromHours(7);
            });
            services.AddIdentity<AppBenutzer, AppRolle>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZçşğüö@!#$%_-1234567890";

                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswortValidator>().AddUserValidator<UserValidator>()
            .AddErrorDescriber<LokalisierungIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbKontext>();

            services.AddScoped<IValidator<AnmeldenAnsichtModell>, BenutzerValidator>();
            services.AddScoped<IValidator<EinloggenAnsichtModell>, EinloggenValidator>();
            services.AddScoped<IValidator<PasswortVergessenAnsichtModell>, PasswortVergessenValidator>();
            services.AddScoped<IValidator<PasswortZurücksetzenAnsichtModell>, PasswortZurücksetzenValidator>();
            services.AddScoped<IEmailDienst, EmailDienst>();
            services.AddValidatorsFromAssemblyContaining<BenutzerValidator>();
        }

    }
}
