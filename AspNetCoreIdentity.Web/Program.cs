using AspNetCoreIdentity.Core.OptionModell;
using AspNetCoreIdentity.Repository.Models;
using AspNetCoreIdentity.Repository.SamenDaten;
using AspNetCoreIdentity.Web.Erweiterungen;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbKontext>(options =>
{
    //! Passwort aus Umgebungsvariable abrufen
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    if (string.IsNullOrEmpty(dbPassword))
    {
        throw new InvalidOperationException(
            "Das Datenbankkennwort wurde in der Umgebungsvariablen nicht gefunden."
        );
    }

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlVerbindung"),
        options =>
        {
            //options.MigrationsAssembly("AspNetCoreIdentity.Repository");
            options.MigrationsAssembly(typeof(AppDbKontext).Assembly.FullName);
        }
    );
    //! Ersetze den Platzhalter {DB_PASSWORD} in der Verbindungszeichenfolge durch das tatsächliche Passwort
    var connectionString = builder.Configuration.GetConnectionString("SqlVerbindung");
    if (connectionString == null)
    {
        throw new InvalidOperationException("Die Verbindungszeichenfolge wurde nicht gefunden.");
    }
    connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
    options.UseSqlServer(
        connectionString,
        options =>
        {
            options.MigrationsAssembly(typeof(AppDbKontext).Assembly.FullName);
        }
    );
});

builder.Services.Configure<EmailEinstellungen>(
    builder.Configuration.GetSection("EmailEinstellungen")
);
builder.Services.AddIdentityMitErweiterung();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
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
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
