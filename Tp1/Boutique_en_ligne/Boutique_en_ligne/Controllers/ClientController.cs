using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using System.Linq;

namespace Boutique_en_ligne.Controllers
{
    public class ClientController : Controller
    {
        // Accès aux vues des pages d'inscription, d'accueil et de la carte de crédit pour alimenter le solde
        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult CarteCredit()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Panier()
        {
            // Récupérer les IDs des jeux depuis la session
            byte[] data = HttpContext.Session.Get("Panier");
            List<int> idsJeux;
            if (data != null && data.Length > 0)
            {
                idsJeux = JsonSerializer.Deserialize<List<int>>(Encoding.UTF8.GetString(data));
            }
            else
            {
                // Si le panier est vide, retourner une vue avec une liste vide
                return View(new List<JeuVideo>());
            }

            // Récupérer les détails des jeux correspondants aux IDs depuis la base de données
            var jeuxDansPanier = _dbContext.JeuVideos.Where(j => idsJeux.Contains(j.Id ?? 0)).ToList();

            // Passer les détails des jeux à la vue
            return View(jeuxDansPanier);
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

            // Redirection vers AddVendeur si le profil est "Vendeur"
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

        // Récupération des informations du client connecté grâce à la session (afin de les afficher dans la vue du profil)
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

        // Mise à jour des informations du client
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

        // Ajout d'une carte de crédit et mise à jour du solde du client
        [HttpPost]
        public IActionResult EnregistrerCarteCredit(Models.CarteCredit carteCredit, float montant)
        {
            string userId = HttpContext.Session.GetString("UserId");

            // Associer la carte de crédit au client connecté
            if (carteCredit != null)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    carteCredit.ClientId = int.Parse(userId); 
                }

                // Vérifier si la carte de crédit existe déjà
                var existingCarteCredit = _dbContext.CarteCredits.FirstOrDefault(cc =>
                    cc.numero == carteCredit.numero &&
                    cc.date == carteCredit.date &&
                    cc.detenteur == carteCredit.detenteur &&
                    cc.numero_secret == carteCredit.numero_secret);

                // Si la carte de crédit n'existe pas, l'ajouter à la base de données
                if (existingCarteCredit == null)
                {
                    _dbContext.CarteCredits.Add(carteCredit);
                    _dbContext.SaveChanges();
                }
            }

            // Mettre à jour le solde du client
            if (!string.IsNullOrEmpty(userId))
            {
                Models.Client client = _dbContext.Clients.FirstOrDefault(u => u.Id == int.Parse(userId));;
                if (client != null)
                {
                    client.solde += montant;
                    _dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Client");
        }

        // Récupérer les jeux vidéos de la BD suite à une recherche sur la page d'accueil
        [HttpGet]
        public IActionResult RechercheJeuxVideo(string titre, string annee_sortie, string console, string genre, string editeur)
        {
            var jeuxVideoParametre = _dbContext.JeuVideos.AsQueryable();
            Console.WriteLine($"Critères de recherche : Titre = {titre}, Année de sortie = {annee_sortie}, Console = {console}, Genre = {genre}, Éditeur = {editeur}");


            if (!string.IsNullOrEmpty(titre))
            {
                jeuxVideoParametre = jeuxVideoParametre.Where(j => j.titre.Contains(titre));
            }

            if (annee_sortie != null)
            {
                jeuxVideoParametre = jeuxVideoParametre.Where(j => j.annee_sortie == annee_sortie);
            }

            if (!string.IsNullOrEmpty(console))
            {
                jeuxVideoParametre = jeuxVideoParametre.Where(j => j.console.Contains(console));
            }

            if (!string.IsNullOrEmpty(genre))
            {
                jeuxVideoParametre = jeuxVideoParametre.Where(j => j.genre.Contains(genre));
            }

            if (!string.IsNullOrEmpty(editeur))
            {
                jeuxVideoParametre = jeuxVideoParametre.Where(j => j.editeur.Contains(editeur));
            }

            var sqlQuery = jeuxVideoParametre.ToQueryString();
            Console.WriteLine($"Requête SQL générée : {sqlQuery}");


            var resultatsRecherche = jeuxVideoParametre.ToList();
            Console.WriteLine($"Nombre de résultats trouvés : {resultatsRecherche.Count}");

            return View("ResultatRecherche", resultatsRecherche);

        }

        [HttpGet]
        public IActionResult AjouterAuPanier(int id)
        {
            var jeuVideo = _dbContext.JeuVideos.FirstOrDefault(j => j.Id == id);

            if (jeuVideo == null)
            {
                return NotFound();
            }

            byte[] data = HttpContext.Session.Get("Panier");

            List<int> idsJeux;
            if (data != null && data.Length > 0)
            {
                idsJeux = JsonSerializer.Deserialize<List<int>>(Encoding.UTF8.GetString(data));
            }
            else
            {
                idsJeux = new List<int>();
            }

            idsJeux.Add(id);
            byte[] newData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(idsJeux));
            HttpContext.Session.Set("Panier", newData);

            return RedirectToAction("Index");
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