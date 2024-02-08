using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class AuthentificationController : Controller
    {
        public IActionResult Authentification()
        {
            return View();
        }
    }
}
