using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Boutique_en_ligne.Controllers
{
    public class ClientController : Controller
    {

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        private readonly BoutiqueJeuDbContext _dbContext;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;

        public ClientController(BoutiqueJeuDbContext dbContext)
        {
            this._dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Utilisateur>();

        }


        [HttpPost]
        public IActionResult AddClient(Models.Client client, string confirmerMotDePasse)
        {
            // Correspondance des mots de passe
            if (client.mot_de_passe!=confirmerMotDePasse)
            {
                ViewBag.ErrorMsg = "Les mots de passe ne correspondent pas";
                return View("~/Views/Home/Inscription.cshtml", client);
            }

            if (client.profil == "Vendeur")
            {
                return RedirectToAction("AddVendeur", "Vendeur", client);
            }
        
            else
            {
                // Hashage du mot de passe
                client.mot_de_passe = _passwordHasher.HashPassword(client, client.mot_de_passe);

                this._dbContext.Clients.Add(client);
                this._dbContext.SaveChanges();

                // Revenir à la page d'accueil après l'inscription
                return RedirectToAction("Index", "Home");
                //return View("ConvertOrAddUser", convertOrAddUser);
            }

        }

        [HttpGet]
        public IActionResult GetClient(string adresse_courriel)
        {
            Models.Client client = _dbContext.Clients.Where(c => c.adresse_courriel == adresse_courriel).First();

            return View();
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