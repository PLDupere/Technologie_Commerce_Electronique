using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Boutique_en_ligne.Controllers
{
    public class ClientController : Controller
    {
        // Accès aux vues des pages d'inscription et d'accueil
        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        // Db context et hashage du mot de passe
        private readonly BoutiqueJeuDbContext _dbContext;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;

        // Constructeur
        public ClientController(BoutiqueJeuDbContext dbContext)
        {
            this._dbContext = dbContext;
            _passwordHasher = new PasswordHasher<Utilisateur>();

        }

        // Ajout d'un client et redirection vers AddVendeur si le profil est "Vendeur"
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
             
            }

        }

        public IActionResult Profil()
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(userId))
            {
                var utilisateur = _dbContext.Clients.FirstOrDefault(u => u.Id == int.Parse(userId));

                if (utilisateur != null && utilisateur.profil == "Client")
                {
                    var client = utilisateur;
                    return View(client);
                }
            }
            return RedirectToAction("Authentification", "Home");
        }

        [HttpGet]
        public IActionResult GetClient(string adresse_courriel)
        {
            Models.Client client = _dbContext.Clients.Where(c => c.adresse_courriel == adresse_courriel).First();

            return View();
        }

        [HttpPost]
        public IActionResult UpdateClient(string confirmerMotDePasse, Models.Client clientToUpdate)
        {
            string userId = HttpContext.Session.GetString("UserId");
            Models.Client client = _dbContext.Clients.FirstOrDefault(u => u.Id == int.Parse(userId));

            if (client != null)
            {
                // Vérifier si les deux mots de passe correspondent
                if (clientToUpdate.mot_de_passe != confirmerMotDePasse)
                {
                    ViewBag.ErrorMsg = "Les mots de passe ne correspondent pas";
                    return View("~/Views/Client/Profil.cshtml", clientToUpdate); 
                }

                clientToUpdate.mot_de_passe = _passwordHasher.HashPassword(clientToUpdate, clientToUpdate.mot_de_passe);

                client.nom = clientToUpdate.nom;
                client.prenom = clientToUpdate.prenom;
                client.date_naissance = clientToUpdate.date_naissance;
                client.ville = clientToUpdate.ville;
                client.mot_de_passe = clientToUpdate.mot_de_passe;


                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", "Client");
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