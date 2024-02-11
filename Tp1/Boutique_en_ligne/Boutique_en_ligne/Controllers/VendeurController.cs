using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Boutique_en_ligne.Controllers
{
    public class VendeurController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;

        public VendeurController(BoutiqueJeuDbContext dbContext)
        {
            this._dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Utilisateur>();

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profil()
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userId))
            {
                var utilisateur = _dbContext.Vendeurs.FirstOrDefault(u => u.Id == int.Parse(userId));

                if (utilisateur != null && utilisateur.profil == "Vendeur")
                {
                    var vendeur = utilisateur;
                    return View(vendeur);
                }
            }
            return RedirectToAction("Authentification", "Home");
        }

        [HttpGet]
        [HttpPost]
        public IActionResult AddVendeur(Models.Vendeur vendeur)
        {
          
            // Hashage du mot de passe
            vendeur.mot_de_passe = _passwordHasher.HashPassword(vendeur, vendeur.mot_de_passe);
           
            this._dbContext.Vendeurs.Add(vendeur);
            this._dbContext.SaveChanges();

            // Revenir à la page d'accueil après l'inscription
            return RedirectToAction("Index", "Home");
            //return View("ConvertOrAddUser", convertOrAddUser);
            

        }

        [HttpGet]
        public IActionResult GetVendeur(string adresse_courriel)
        {
            Models.Vendeur vendeur = _dbContext.Vendeurs.Where(v => v.adresse_courriel == adresse_courriel).First();

            return View(vendeur);
        }

        [HttpPost]
        public IActionResult UpdateVendeur(string confirmerMotDePasse, Models.Vendeur vendeurToUpdate)
        {
            string userId = HttpContext.Session.GetString("UserId");
            Models.Vendeur vendeur = _dbContext.Vendeurs.FirstOrDefault(u => u.Id == int.Parse(userId));

            if (vendeur != null)
            {
                // Vérifier si les deux mots de passe correspondent
                if (vendeurToUpdate.mot_de_passe != confirmerMotDePasse)
                {
                    ViewBag.ErrorMsg = "Les mots de passe ne correspondent pas";
                    return View("~/Views/Vendeur/Profil.cshtml", vendeurToUpdate);
                }

                vendeurToUpdate.mot_de_passe = _passwordHasher.HashPassword(vendeurToUpdate, vendeurToUpdate.mot_de_passe);

                vendeur.nom = vendeurToUpdate.nom;
                vendeur.prenom = vendeurToUpdate.prenom;
                vendeur.date_naissance = vendeurToUpdate.date_naissance;
                vendeur.ville = vendeurToUpdate.ville;
                vendeur.mot_de_passe = vendeurToUpdate.mot_de_passe;


                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Vendeur");
        }

        [HttpDelete]
        public IActionResult DeleteVendeur(int vendeurId)
        {
            Models.Vendeur vendeur = _dbContext.Vendeurs.Where(v => v.Id == vendeurId).First();
            _dbContext.Vendeurs.Remove(vendeur);
            _dbContext.SaveChanges();

            return View();
        }
    }
}