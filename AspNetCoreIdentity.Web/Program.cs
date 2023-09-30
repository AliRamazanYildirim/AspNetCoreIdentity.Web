using AspNetCoreIdentity.Web.Erweiterungen;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Core.OptionModell;
using AspNetCoreIdentity.Web.SamenDaten;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbKontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlVerbindung"));
});


builder.Services.Configure<EmailEinstellungen>(builder.Configuration.GetSection("EmailEinstellungen"));
builder.Services.AddIdentityMitErweiterung();

var app = builder.Build();

using(var scope=app.Services.CreateAsyncScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRolle>>();
    await BerechtigungSamenDaten.Samen(roleManager);
}

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
