using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class ClientController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public ClientController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddClient(Models.Client client)
        {
            _dbContext.Clients.Add(client);
            _dbContext.SaveChanges();

            return View();
            //return View("ConvertOrAddUser", convertOrAddUser);
        }

        [HttpGet]
        public IActionResult GetClient(string adresse_courriel)
        {
            Models.Client client = _dbContext.Clients.Where(c => c.adresse_courriel == adresse_courriel).First();

            return View(client);
        }

        [HttpPut]
        public IActionResult UpdateClient(int clientId, Models.Client clientToUpdate)
        {
            Models.Client client = _dbContext.Clients.Where(c => c.Id == clientId).First();
            client = clientToUpdate;
            _dbContext.SaveChanges();

            return View(client);
        }

        [HttpDelete]
        public IActionResult DeleteClient(int clientId)
        {
            Models.Client client = _dbContext.Clients.Where(c => c.Id == clientId).First();
            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();

            return View();
        }
    }
}
