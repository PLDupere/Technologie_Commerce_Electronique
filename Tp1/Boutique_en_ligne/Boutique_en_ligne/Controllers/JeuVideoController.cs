using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Boutique_en_ligne.Controllers
{
    public class JeuVideoController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        //private readonly IHttpClientFactory _clientFactory;  //Install-Package System.Net.Http.Json -Version 7.0.0


        public JeuVideoController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
            //_clientFactory = clientFactory;
        }
        public IActionResult Ajouter()
        {
            return View();
        }

        public IActionResult Modifier()
        {
            List<Models.JeuVideo> jeuVideos = this._dbContext.JeuVideos.ToList();
            return View(jeuVideos);
        }

        public IActionResult MiseAJour(int id)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.FirstOrDefault(j => j.Id == id);

            if (jeuVideo == null)
            {
                return NotFound();
            }

            return View(jeuVideo);
        }

        public IActionResult Recherche()
        {
            return View();
        }

        public IActionResult Afficher(Models.JeuVideo jeu)
        {
            return View(jeu);
        }

        [HttpPost]
        public IActionResult AddJeuVideo(Models.JeuVideo jeuVideo)
        {
            _dbContext.JeuVideos.Add(jeuVideo);
            _dbContext.SaveChanges();

            return RedirectToAction("Modifier", "JeuVideo");
        }

        [HttpGet]
        public IActionResult GetJeuVideo(int jeuVideoId)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == jeuVideoId).First();

            if (jeuVideo == null)
            {
                return NotFound();
            }

            return RedirectToAction("Modifier", "JeuVideo");
        }

        [HttpGet]
        public IActionResult GetJeuxVideos()
        {
            List<Models.JeuVideo> jeuxVideos = _dbContext.JeuVideos.ToList();

            return View(jeuxVideos);
        }

        [HttpPost]
        public IActionResult UpdateJeuVideo(int Id, Models.JeuVideo jeuVideoToUpdate)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == Id).First();
            Debug.WriteLine($"Before Update - Id: {jeuVideo.Id}");
            Debug.WriteLine($"IDDDD {Id}");
            jeuVideo = jeuVideoToUpdate;
            _dbContext.SaveChanges();

            return RedirectToAction("Modifier", "JeuVideo");
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("JeuVideo/DeleteJeuVideo/{Id:int}")]
        public IActionResult DeleteJeuVideo(int Id)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == Id).First();
            if (jeuVideo != null)
            {
                _dbContext.JeuVideos.Remove(jeuVideo);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Modifier", "JeuVideo");
        }

        // ***** API *****

        private readonly string apiKey = "755e1f2dda34491da4ac33116d2608d0";
        private readonly string apiUrlBase = "https://api.rawg.io/api/games";

        [HttpPost]
        public async Task<IActionResult> RechercheAPI(string searchName)
        {
            Models.JeuVideo jeuVideoToReturn = new Models.JeuVideo();

            string apiUrl = $"{apiUrlBase}?key={apiKey}&search={searchName}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                System.Diagnostics.Debug.WriteLine($"HTTP Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    JObject json = JObject.Parse(jsonResponse);

                    if (json != null && json["results"] != null)
                    {

                        var firstResult = json["results"][0];

                        //good
                        jeuVideoToReturn.titre = (string)firstResult["name"];
                        jeuVideoToReturn.annee_sortie = (string)firstResult["released"];
                        jeuVideoToReturn.pochette_jeu = (string)firstResult["background_image"]; 
                        jeuVideoToReturn.console = (string)firstResult?["platforms"]?[0]?["platform"]?["name"];

                        jeuVideoToReturn.genre = (string)firstResult?["tags"]?[0]?["name"];
                        jeuVideoToReturn.editeur = (string)firstResult["stores"]?[0]?["store"]?["name"];
                        jeuVideoToReturn.capture_ecran = (string)firstResult["short_screenshots"]?[0]?["image"];

                        return RedirectToAction("Afficher", "JeuVideo", jeuVideoToReturn);
                    }
                    else
                    {
                        return RedirectToAction("Recherche", "JeuVideo");
                    }
                }
                else
                {
                    return RedirectToAction("Recherche", "JeuVideo");
                }
            }
        }
    }
}