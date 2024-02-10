

using Boutique_en_ligne;
using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddDbContext<BoutiqueJeuDbContext>();
builder.Services.AddScoped<IPasswordHasher<Utilisateur>, PasswordHasher<Utilisateur>>();

var app = builder.Build();


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
});


app.UseFileServer();

app.Run();
