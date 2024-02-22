using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Boutique_en_ligne.Controllers
{
    public class VendeurController : Controller
    {
        // DbContext et PasswordHasher
        private readonly BoutiqueJeuDbContext _dbContext;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;

        // Constructeur
        public VendeurController(BoutiqueJeuDbContext dbContext)
        {
            this._dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Utilisateur>();

        }

        // Vue pour la page d'accueil
        public IActionResult Index()
        {
            return View();
        }

        // Récupération des informations du vendeur connecté grâce à la session (afin de les afficher dans la vue du profil)
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

        public IActionResult Statistiques()
        {
            var vendeurId = HttpContext.Session.GetString("UserId");
            int vendeurIdInt = int.Parse(vendeurId);

            // Calculer le montant total des ventes pour le vendeur connecté
            float montantTotalVentesVendeur = (float)_dbContext.Factures
                .Where(f => f.JeuxVideos.Any(j => j.vendeurId == vendeurIdInt))
                .Sum(f => f.montant_total);

            // Nombre total de jeux vendus par le vendeur
            int nombreTotalJeuxVendus = _dbContext.Factures
                .Where(f => f.JeuxVideos.Any(j => j.vendeurId == vendeurIdInt))
                .SelectMany(f => f.JeuxVideos)
                .Count();

            // Nombre de clients ayant acheté les jeux du vendeur
            int nombreClientsAchetantJeuxVendeur = _dbContext.JeuVideos
                .Where(j => j.vendeurId == vendeurIdInt)
                .SelectMany(j => j.Facture.JeuxVideos.Select(fj => fj.Utilisateur.Id))
                .Distinct()
                .Count();

            ViewBag.MontantTotalVentes = montantTotalVentesVendeur;
            ViewBag.NombreTotalJeuxVendus = nombreTotalJeuxVendus;
            ViewBag.NombreClientsAchetantJeux = nombreClientsAchetantJeuxVendeur;

            return View();
        }

        // Inscription d'un vendeur
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
        }

        [HttpGet]
        public IActionResult GetVendeur(string adresse_courriel)
        {
            Models.Vendeur vendeur = _dbContext.Vendeurs.Where(v => v.adresse_courriel == adresse_courriel).First();

            return View(vendeur);
        }

        // Mise à jour des informations du vendeur 
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

        public IActionResult Afficher()
        {
           
            var vendeurId = HttpContext.Session.GetString("UserId");
            
            // Récupérer les jeux vidéos du vendeur connecté
            List<Models.JeuVideo> jeuVideos = this._dbContext.JeuVideos.Where(j => j.vendeurId == int.Parse(vendeurId)).ToList();
            return View(jeuVideos);
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