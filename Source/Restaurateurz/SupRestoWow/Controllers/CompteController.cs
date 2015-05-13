using SupRestoWow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupRestoWow.Controllers
{
    public class CompteController : Controller
    {
        // GET: Compte
        [HttpGet]
        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(Compte client)
        {
            return null;
        }
    }
}