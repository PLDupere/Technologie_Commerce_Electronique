using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Boutique_en_ligne.Controllers
{
    public class HomeController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;
        

        public HomeController(BoutiqueJeuDbContext dbContext)
        {
            this._dbContext = dbContext;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Connecter(Utilisateur utilisateur, string mdp)
        {
            var utilisateurConnecte = _dbContext.Utilisateurs.FirstOrDefault(u => u.adresse_courriel == utilisateur.adresse_courriel);
            if (utilisateurConnecte != null)
            {
                // Vérifiez si le mot de passe fourni correspond au mot de passe haché stocké dans la base de données
                var passwordHasher = new PasswordHasher<Utilisateur>();
                var result = passwordHasher.VerifyHashedPassword(utilisateurConnecte, utilisateurConnecte.mot_de_passe, mdp);
                // montrer mdp haché
                Console.WriteLine(utilisateurConnecte.mot_de_passe);
                Console.WriteLine(result);



                if (result == PasswordVerificationResult.Success)
                {
                    // Authentification réussie, enregistrez les informations de l'utilisateur dans la session
                    HttpContext.Session.SetString("UserId", utilisateurConnecte.Id.ToString());
                    HttpContext.Session.SetString("UserProfil", utilisateurConnecte.profil.ToString());

                    // Redirigez l'utilisateur vers une page appropriée en fonction de son profil
                    if (utilisateurConnecte.profil == "Client")
                    {
                        //faire un log dans la console
                        Console.WriteLine("Client connecté");

                        return RedirectToAction("Index", "Client");
                    }
                    else if (utilisateurConnecte.profil == "Vendeur")
                    {

                        Console.WriteLine("Vendeur connecté");
                        return RedirectToAction("Index", "Vendeur");
                    }
                }
          
            }

            // Authentification échouée, affichez un message d'erreur
            ViewBag.ErrorMessageConnexion = "Adresse e-mail ou mot de passe incorrect.";
            return View("Authentification"); // Afficher à nouveau le formulaire de connexion
        }
    }
}
