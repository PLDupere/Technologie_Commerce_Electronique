using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class FactureController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public FactureController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddFacture(Models.Facture facture)
        {
            _dbContext.Factures.Add(facture);
            _dbContext.SaveChanges();

            return View();
            //return View("ConvertOrAddUser", convertOrAddUser);
        }

        [HttpGet]
        public IActionResult GetFacture(int factureId)
        {
            Models.Facture facture = _dbContext.Factures.Where(f => f.Id == factureId).First();

            return View(facture);
        }

        [HttpGet]
        public IActionResult GetFactures()
        {
            List<Models.Facture> factures = _dbContext.Factures.ToList();

            return View(factures);
        }

        [HttpPut]
        public IActionResult UpdateFacture(int factureId, Models.Facture factureToUpdate)
        {
            Models.Facture facture = _dbContext.Factures.Where(f => f.Id == factureId).First();
            facture = factureToUpdate;
            _dbContext.SaveChanges();

            return View(facture);
        }

        [HttpDelete]
        public IActionResult DeleteFacture(int factureId)
        {
            Models.Facture facture = _dbContext.Factures.Where(f => f.Id == factureId).First();
            _dbContext.Factures.Remove(facture);
            _dbContext.SaveChanges();

            return View();
        }
    }
}