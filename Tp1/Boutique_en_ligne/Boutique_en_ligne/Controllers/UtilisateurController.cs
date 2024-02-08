using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public abstract class UtilisateurController : Controller
    {
        private readonly BoutiqueJeuDbContext _dbContext;

        public UtilisateurController(BoutiqueJeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Inutile ...
    }
}
