using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class VendeurController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public VendeurController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddVendeur(Models.Vendeur vendeur)
        {
            _dbContext.Vendeurs.Add(vendeur);
            _dbContext.SaveChanges();

            return View();
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
