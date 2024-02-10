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

        [HttpPut]
        public IActionResult UpdateVendeur(int vendeurId, Models.Vendeur vendeurToUpdate)
        {
            Models.Vendeur vendeur = _dbContext.Vendeurs.Where(v => v.Id == vendeurId).First();
            vendeur = vendeurToUpdate;
            _dbContext.SaveChanges();

            return View(vendeur);
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