using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Boutique_en_ligne.Controllers
{
    public class JeuVideoController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;


        public JeuVideoController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult AjouterAPI(Models.JeuVideo jeu)
        {
            Models.JeuVideo jeuVideo = new Models.JeuVideo();

            jeuVideo.titre = jeu.titre;
            jeuVideo.annee_sortie = jeu.annee_sortie;
            jeuVideo.console = jeu.console?.ToString();
            jeuVideo.editeur = jeu.editeur?.ToString();
            jeuVideo.genre = jeu.genre?.ToString();
            jeuVideo.pochette_jeu = jeu.pochette_jeu;
            jeuVideo.capture_ecran = jeu.capture_ecran?.ToString();
            jeuVideo.prix_vente = jeu.prix_vente;


            _dbContext.JeuVideos.Add(jeuVideo);
            _dbContext.SaveChanges();

            return RedirectToAction("Modifier", "JeuVideo");
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
        
        [HttpGet]
        public IActionResult Afficher()
        {
            if (TempData.TryGetValue("JeuVideoListJson", out object jeuVideoListJsonObj) && jeuVideoListJsonObj is string jeuVideoListJson)
            {
                List<Models.Afficher> jeuVideoList = JsonConvert.DeserializeObject<List<Models.Afficher>>(jeuVideoListJson);
                return View(jeuVideoList);
            }
            else
            {
                return RedirectToAction("Recherche", "JeuVideo");
            }
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
            List<Models.Afficher> jeuVideoList = new List<Models.Afficher>();

            string apiUrl = $"{apiUrlBase}?key={apiKey}&search={searchName}&page_size=10";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                //HTTP request-response cycle where the server is indicating that the request header fields are too large (status code 431). 
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    JObject json = JObject.Parse(jsonResponse);

                    if (json != null && json["results"] != null)
                    {
                        foreach (var result in json["results"])
                        {
                            Models.Afficher jeuVideoToReturn = new Models.Afficher();

                            jeuVideoToReturn.titre = GetValueOrDefault((string)result["name"]);
                            jeuVideoToReturn.annee_sortie = GetValueOrDefault((string)result["released"]);
                            jeuVideoToReturn.pochette_jeu = GetValueOrDefault((string)result["background_image"]);

                            JToken platformsToken = result?["platforms"];
                            jeuVideoToReturn.console = platformsToken != null && platformsToken.HasValues
                                ? platformsToken.Select(p => GetValueOrDefault((string)p["platform"]["name"])).ToArray()
                                : new string[0];

                            JToken tagsToken = result?["tags"];
                            jeuVideoToReturn.genre = tagsToken != null && tagsToken.HasValues
                                ? tagsToken.Select(t => GetValueOrDefault((string)t["name"])).ToArray()
                                : new string[0];

                            JToken storesToken = result?["stores"];
                            jeuVideoToReturn.editeur = storesToken != null && storesToken.HasValues
                                ? storesToken.Select(s => GetValueOrDefault((string)s["store"]["name"])).ToArray()
                                : new string[0];

                            JToken screenshotsToken = result["short_screenshots"];
                            jeuVideoToReturn.capture_ecran = screenshotsToken != null && screenshotsToken.HasValues
                                ? screenshotsToken.Select(s => GetValueOrDefault((string)s["image"])).ToArray()
                                : new string[0];

                            jeuVideoList.Add(jeuVideoToReturn);
                        }
                        string jeuVideoListJson = JsonConvert.SerializeObject(jeuVideoList);
                        TempData["JeuVideoListJson"] = jeuVideoListJson;
                        return RedirectToAction("Afficher", "JeuVideo");
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
        private string GetValueOrDefault(string value, string defaultValue = "S/O")
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}