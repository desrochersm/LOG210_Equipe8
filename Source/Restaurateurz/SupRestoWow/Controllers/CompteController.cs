using SupRestoWow.DataStore;
using SupRestoWow.Auth;
using SupRestoWow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupRestoWow.Controllers
{
    /// <summary>
    /// Controleur de gestion des comptes
    /// </summary>
    public class CompteController : Controller
    {
        /// <summary>
        /// Créer un nouveau compte
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Creer(Compte compte)
        {
            //Va lancer une exception jusqu'à ce que l'on ait une BD
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

        /// <summary>
        /// Obtention de la vue d'authentification
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Authentification()
        {
            //On gère pas pour l'instant si une personne est connectée et qu'elle revient sur l'authentification
            return View();
        }

        /// <summary>
        /// Authentifie un utilisateur
        /// </summary>
        /// <param name="compte">Compte à authentifier</param>
        /// <returns></returns>
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

                Guid cleSession = CacheSession.Instance.AjouterSession(compte);
                HttpContext.Response.Cookies.Add(new HttpCookie("session", cleSession.ToString()));

                //Ajouter notre de où on s'en va!
                return View();//Redirect("");
            }
        }

        /// <summary>
        /// Déconnexion d'un utilisateur
        /// </summary>
        /// <returns></returns>
        [HttpGet,AuthorizedAttribute]
        public ActionResult Deconnexion()
        {
            //Believe me or not, en changeant l'expiration du cookie c'est la seule façon de le supprimer
            string cleSession = HttpContext.Request.Cookies["session"].Value;
            HttpCookie cookie = new HttpCookie("session", "");
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(cookie);
            CacheSession.Instance.RetirerSession(Guid.Parse(cleSession));
            return View("Authentification");
        }

        /// <summary>
        /// C'est temporaire
        /// </summary>
        /// <returns></returns>
        [HttpGet,AuthorizedAttribute]
        public string text()
        { return "Yo dawg"; }
    }
}