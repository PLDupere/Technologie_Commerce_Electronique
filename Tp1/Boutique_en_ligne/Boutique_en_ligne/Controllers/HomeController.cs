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

        // Vues pour les pages d'accueil, d'inscription et d'authentification
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

        // Connexion d'un utilisateur et enrégistrement de ses informations dans la session
        [HttpPost]
        public IActionResult Connecter(Utilisateur utilisateur, string mdp)
        {
            var utilisateurConnecte = _dbContext.Utilisateurs.FirstOrDefault(u => u.adresse_courriel == utilisateur.adresse_courriel);
            if (utilisateurConnecte != null)
            {
                // Vérifier si le mot de passe fourni correspond au mot de passe haché stocké dans la base de données
                var passwordHasher = new PasswordHasher<Utilisateur>();
                var result = passwordHasher.VerifyHashedPassword(utilisateurConnecte, utilisateurConnecte.mot_de_passe, mdp);

                if (result == PasswordVerificationResult.Success)
                {
                    // Enregistrer l'ID et le profil de l'utilisateur dans la session
                    HttpContext.Session.SetString("UserId", utilisateurConnecte.Id.ToString());
                    HttpContext.Session.SetString("UserProfil", utilisateurConnecte.profil.ToString());

                    // Rediriger l'utilisateur vers la page appropriée en fonction de son profil
                    if (utilisateurConnecte.profil == "Client")
                    {
                        return RedirectToAction("Index", "Client");
                    }
                    else if (utilisateurConnecte.profil == "Vendeur")
                    {
                        return RedirectToAction("Index", "Vendeur");
                    }
                }
          
            }

            // Message d'erreur si l'authentification échoue
            ViewBag.ErrorMessageConnexion = "Adresse e-mail ou mot de passe incorrect.";
            return View("Authentification"); // Afficher à nouveau le formulaire de connexion
        }
    }
}
