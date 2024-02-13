using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class JeuVideoController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public JeuVideoController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult AddJeuVideo(Models.JeuVideo jeuVideo)
        {
            _dbContext.JeuVideos.Add(jeuVideo);
            _dbContext.SaveChanges();

            return View();
            //return View("ConvertOrAddUser", convertOrAddUser);
        }

        [HttpGet]
        public IActionResult GetJeuVideo(int jeuVideoId)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == jeuVideoId).First();

            return View(jeuVideo);
        }

        [HttpGet]
        public IActionResult GetJeuxVideos()
        {
            List<Models.JeuVideo> jeuxVideos = _dbContext.JeuVideos.ToList();

            return View(jeuxVideos);
        }

        [HttpPut]
        public IActionResult UpdateJeuVideo(int jeuVideoId, Models.JeuVideo jeuVideoToUpdate)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == jeuVideoId).First();
            jeuVideo = jeuVideoToUpdate;
            _dbContext.SaveChanges();

            return View(jeuVideo);
        }

        [HttpDelete]
        public IActionResult DeleteJeuVideo(int jeuVideoId)
        {
            Models.JeuVideo jeuVideo = _dbContext.JeuVideos.Where(j => j.Id == jeuVideoId).First();
            _dbContext.JeuVideos.Remove(jeuVideo);
            _dbContext.SaveChanges();

            return View();
        }
    }
}