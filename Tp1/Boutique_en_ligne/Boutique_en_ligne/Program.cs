

using Boutique_en_ligne;
using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddDbContext<BoutiqueJeuDbContext>();
builder.Services.AddScoped<IPasswordHasher<Utilisateur>, PasswordHasher<Utilisateur>>();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseSession();


app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "Inscription", 
        template: "Home/Inscription", 
        defaults: new { controller = "Home", action = "Inscription" } 
    );


    routes.MapRoute(
        name: "Authentification", 
        template: "Home/Authentification", 
        defaults: new { controller = "Home", action = "Authentification" } 
    );

    routes.MapRoute(
        name: "Default",
        template: "{controller=Home}/{action=Index}");

    routes.MapRoute(
        name: "ProfilClient",
        template: "Client/Profil",
        defaults: new { controller = "Client", action = "Profil" }
    );
});


app.UseFileServer();

app.Run();
