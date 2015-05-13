using SupRestoWow.DataStore;
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
        [HttpPost]
        public ActionResult Creer(Compte compte)
        {
            using (RestaurentContext context = new RestaurentContext())
            {
                bool compteExiste = context.Set<Compte>().Any(c => c.Nom == compte.Nom);

                if (compteExiste)
                {
                    ModelState.AddModelError("", "Le compte existe déjà");
                    return View(compte);
                }

                context.Set<Compte>().Add(compte);

                //Quand on va avoir une database
                //context.SaveChanges();
            }
            return null;
        }

        // GET: Compte
        [HttpGet]
        public ActionResult Authentification()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authentification(Compte compte)
        {
            using (RestaurentContext context = new RestaurentContext())
            {
                //Décommenter lorsque l'on aura une BD!

                //Compte compteBd = context.Set<Compte>().SingleOrDefault(c => compte.Nom == c.Nom);
                //if(compteBd.MotDePasse != compte.MotDePasse)
                //{
                //    ModelState.AddModelError("", "Le nom de compte ou le mot de passe est invalide");
                //    return View(compte);
                //}

                Guid cleSession = CacheSession.Instance.AjouterCompte(compte);
                HttpContext.Response.Cookies.Add(new HttpCookie("session", cleSession.ToString()));

                //Ajouter notre de où on s'en va!
                return View();//Redirect("");
            }
        }

        [HttpGet,Authorized]
        public ActionResult Deconnexion()
        {
            //Marche pas, oops
            HttpContext.Response.Cookies.Remove("session");
            return View("Authentification");
        }

        [HttpGet,Authorized]
        public string text()
        { return "Yo dawg"; }
    }
}