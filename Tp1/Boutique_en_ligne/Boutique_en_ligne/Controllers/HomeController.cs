﻿using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Authentification()
        {
            return View();
        }

    }
}
