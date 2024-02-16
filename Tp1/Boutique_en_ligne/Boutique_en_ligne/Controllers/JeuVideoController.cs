using Boutique_en_ligne.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Boutique_en_ligne.Controllers
{
    public class JeuVideoController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public JeuVideoController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}