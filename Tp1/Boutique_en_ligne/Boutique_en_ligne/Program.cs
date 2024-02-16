using Boutique_en_ligne;
using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Services de l'application
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddDbContext<BoutiqueJeuDbContext>();
builder.Services.AddScoped<IPasswordHasher<Utilisateur>, PasswordHasher<Utilisateur>>();    // Pour hasher les mots de passe
builder.Services.AddSession();                                                              // Authentification par session
builder.Services.AddControllersWithViews();                                                 
builder.Services.AddHttpContextAccessor();                                                  // Pour accéder à la session


var app = builder.Build();
app.UseSession();

// Routes (boutons de la barre de navigation)
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

    routes.MapRoute(
        name: "Solde",
        template: "Client/CarteCredit",
        defaults: new { controller = "Client", action = "CarteCredit" }
        );

    routes.MapRoute(
      name: "ProfilVendeur",
      template: "Vendeur/Profil",
      defaults: new { controller = "Vendeur", action = "Profil" }
      );

    routes.MapRoute(
     name: "AccueilClient",
     template: "Client/Index",
     defaults: new { controller = "Client", action = "Index" }
     );

    routes.MapRoute(
     name: "AccueilVendeur",
     template: "Vendeur/Index",
     defaults: new { controller = "Vendeur", action = "Index" }
     );

    routes.MapRoute(
     name: "Panier",
     template: "Client/Panier",
     defaults: new { controller = "Client", action = "Panier" }
     );


});


app.UseFileServer();

app.Run();
