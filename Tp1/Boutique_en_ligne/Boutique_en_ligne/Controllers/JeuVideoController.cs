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

        public IActionResult Afficher(List<Models.JeuVideo> jeuVideoList)
        {
            return View(jeuVideoList);
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
            List<Models.JeuVideo> jeuVideoList = new List<Models.JeuVideo>();

            string apiUrl = $"{apiUrlBase}?key={apiKey}&search={searchName}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    JObject json = JObject.Parse(jsonResponse);

                    if (json != null && json["results"] != null)
                    {
                        foreach (var result in json["results"])
                        {
                            Models.JeuVideo jeuVideoToReturn = new Models.JeuVideo();

                            jeuVideoToReturn.titre = (string)result["name"];
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
                                ? storesToken.Select(s => GetValueOrDefault((string)s["name"])).ToArray()
                                : new string[0];

                            JToken screenshotsToken = result["short_screenshots"];
                            jeuVideoToReturn.capture_ecran = screenshotsToken != null && screenshotsToken.HasValues
                                ? screenshotsToken.Select(s => GetValueOrDefault((string)s["image"])).ToArray()
                                : new string[0];

                            jeuVideoList.Add(jeuVideoToReturn);
                        }

                        return RedirectToAction("Afficher", new { Capacity = 5, Count = jeuVideoList.Count, jeuVideoList });
                        //return RedirectToAction("Afficher", new { Capacity = 5, Count = jeuVideoList.Count, jeuVideoListJson = JsonConvert.SerializeObject(jeuVideoList) });
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