using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.ClaimProviders;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.OptionModell;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbKontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlVerbindung"));
});


builder.Services.Configure<EmailEinstellungen>(builder.Configuration.GetSection("EmailEinstellungen"));
builder.Services.AddIdentityMitErweiterung();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminStadtPolicy", policy =>
    {
        policy.RequireClaim("stadt", "Frankfurt");
    });
});
builder.Services.ConfigureApplicationCookie(conf =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
