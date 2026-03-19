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
    var connectionString = builder.Configuration.GetConnectionString("SqlVerbindung");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Die Verbindungszeichenfolge wurde nicht gefunden.");
    }

    const string dbPasswordPlaceholder = "{DB_PASSWORD}";
    if (connectionString.Contains(dbPasswordPlaceholder, StringComparison.Ordinal))
    {
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        if (string.IsNullOrWhiteSpace(dbPassword))
        {
            throw new InvalidOperationException(
                "Das Datenbankkennwort wurde in der Umgebungsvariablen DB_PASSWORD nicht gefunden."
            );
        }

        connectionString = connectionString.Replace(
            dbPasswordPlaceholder,
            dbPassword,
            StringComparison.Ordinal
        );
    }

    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AppDbKontext).Assembly.FullName);
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
