using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class CarteCreditController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public CarteCreditController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddCreditCarte(Models.CarteCredit carteCredit)
        {
            _dbContext.CarteCredits.Add(carteCredit);
            _dbContext.SaveChanges();

            return View();
            //return View("ConvertOrAddUser", convertOrAddUser);
        }

        [HttpGet]
        public IActionResult GetCreditCart(int clientId)
        {
            Models.CarteCredit carteCredit = _dbContext.CarteCredits.Where(c => c.Id == clientId).First();

            return View(carteCredit);
        }

        [HttpPut]
        public IActionResult UpdateCreditCart(int clientId ,Models.CarteCredit carteCreditToUpdate)
        {
            Models.CarteCredit carteCredit = _dbContext.CarteCredits.Where(c => c.Id == clientId).First();
            carteCredit = carteCreditToUpdate;
            _dbContext.SaveChanges();

            return View(carteCredit);
        }

        [HttpDelete]
        public IActionResult DeleteCreditCart(int clientId)
        {
            Models.CarteCredit carteCredit = _dbContext.CarteCredits.Where(c => c.Id == clientId).First();
            _dbContext.CarteCredits.Remove(carteCredit);
            _dbContext.SaveChanges();

            return View();
        }
    }
}
