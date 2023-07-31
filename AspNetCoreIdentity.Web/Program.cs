using AspNetCoreIdentity.Web.AnsichtModelle;
using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.FluentValidierer;
using AspNetCoreIdentity.Web.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbKontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlVerbindung"));
});

builder.Services.AddScoped<IValidator<AnmeldenAnsichtModell>, BenutzerValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<BenutzerValidator>();

builder.Services.AddIdentityMitErweiterung();

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
