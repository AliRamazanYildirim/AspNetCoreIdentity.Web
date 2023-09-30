using AspNetCoreIdentity.Web.Anforderungen;
using AspNetCoreIdentity.Core.AnsichtModelle;
using AspNetCoreIdentity.Web.Areas.Admin.FluentValidierer;
using AspNetCoreIdentity.Web.ClaimProviders;
using AspNetCoreIdentity.Web.Dienste;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Lokalisierungen;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Core.OptionModell;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Configuration;
using AspNetCoreIdentity.Core.Models;
using AspNetCoreIdentity.Core.FluentValidierer;

namespace AspNetCoreIdentity.Web.Erweiterungen
{
    public static class InbetriebnahmeErweiterung
    {
        public static void AddIdentityMitErweiterung(this IServiceCollection services)
        {
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(30);
            });
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

            services.AddScoped<IEmailDienst, EmailDienst>();

            services.AddScoped<IValidator<AnmeldenAnsichtModell>, BenutzerValidator>();
            services.AddScoped<IValidator<EinloggenAnsichtModell>, EinloggenValidator>();
            services.AddScoped<IValidator<PasswortVergessenAnsichtModell>, PasswortVergessenValidator>();
            services.AddScoped<IValidator<PasswortZurücksetzenAnsichtModell>, PasswortZurücksetzenValidator>();
            services.AddScoped<IValidator<PasswortÄndernAnsichtsModell>, PasswortÄndernValidator>();
            services.AddScoped<IValidator<BenutzerBearbeitenAnsichtModell>, BenutzerBearbeitenValidator>();
            services.AddValidatorsFromAssemblyContaining<BenutzerValidator>();
            services.AddValidatorsFromAssemblyContaining<RolleValidator>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            services.AddHttpContextAccessor();
            services.AddScoped<IClaimsTransformation, UserClaimProvider>();
            services.AddScoped<IAuthorizationHandler, UmtauschVerfallsAnforderungHandler>();
            services.AddScoped<IAuthorizationHandler, GewaltAnforderungHandler>();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminStadtPolicy", policy =>
                {
                    policy.RequireClaim("stadt", "Frankfurt");
                });
                opt.AddPolicy("UmtauschPolicy", policy =>
                {
                    policy.AddRequirements(new UmtauschVerfallsAnforderung());
                });
                opt.AddPolicy("GewaltPolicy", policy =>
                {
                    policy.AddRequirements(new GewaltAnforderung()
                    {
                        Alter = 18
                    });
                });
                opt.AddPolicy("BestellungBerechtigungLesenOderLöschenPolicy", policy =>
                {
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Bestellung.Lesen);
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Katalog.Löschen);
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Vorrat.Löschen);
                });
                opt.AddPolicy("BerechtigungenRoot.Berechtigungen.Bestellung.Lesen", policy =>
                {
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Bestellung.Lesen);
                });
                opt.AddPolicy("BerechtigungenRoot.Berechtigungen.Bestellung.Löschen", policy =>
                {
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Bestellung.Löschen);
                });
                opt.AddPolicy("BerechtigungenRoot.Berechtigungen.Vorrat.Löschen", policy =>
                {
                    policy.RequireClaim("Berechtigungen", Core.BerechtigungenRoot.Berechtigungen.Vorrat.Löschen);
                });
            });
            services.ConfigureApplicationCookie(conf =>
            {
                var cookieBuilder = new CookieBuilder
                {
                    Name = "IdentityCookie"
                };
                conf.LoginPath = new PathString("/Home/Einloggen");
                conf.LogoutPath = new PathString("/Mitglied/Ausloggen");
                conf.AccessDeniedPath = new PathString("/Mitglied/AccessDenied");
                conf.Cookie = cookieBuilder;
                conf.ExpireTimeSpan = TimeSpan.FromDays(30);
                conf.SlidingExpiration = true;
            });
        }
    }
}
