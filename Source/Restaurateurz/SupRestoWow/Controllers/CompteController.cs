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
        /// Afficher la vue pour créer un nouveau compte
        /// </summary>
        /// <returns></returns>
        public ActionResult Creer()
        {
            return View("CreationCompte");
        }

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
                bool compteExiste = context.Set<Compte>().Any(c => c.Courriel == compte.Courriel);

                if (compteExiste)
                {
                    ModelState.AddModelError("", "Le compte existe déjà");
                    return View(compte);
                }

                context.Set<Compte>().Add(compte);
                context.SaveChanges();
            }

            //On retourne où maintenant?
            return null;
        }

        /// <summary>
        /// Afficher la vue pour modifier un compte
        /// </summary>
        /// <returns></returns>
        public ActionResult Modifier()
        {
            return View("ModifierCompte");
        }

        /// <summary>
        /// Modifier un compte
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modifier(Compte compte)
        {
            Compte compteInBD;
            //Va lancer une exception jusqu'à ce que l'on ait une BD
            using (RestaurentContext context = new RestaurentContext())
            {
                compteInBD = context.Set<Compte>().Where(c => c.Courriel == compte.Courriel).First();
            }

            if(compteInBD == null){
                ModelState.AddModelError("", "Le compte n'a pas été trouvé dans la BD");
                return View(compte);
            }

            compteInBD = compte;

            using (RestaurentContext context = new RestaurentContext())
            {
                context.Entry(compteInBD).State = System.Data.EntityState.Modified;
                context.SaveChanges();
            }

            //On retourne où maintenant?
            return null;
        }

        /// <summary>
        /// Obtention de la vue d'authentification
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Connexion()
        {
            //On gère pas pour l'instant si une personne est connectée et qu'elle revient sur l'authentification
            return View("Authentification");
        }

        /// <summary>
        /// Authentifie un utilisateur
        /// </summary>
        /// <param name="compte">Compte à authentifier</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Connexion(Compte compte)
        {
            using (RestaurentContext context = new RestaurentContext())
            {
                Compte compteBd = context.Set<Compte>().SingleOrDefault(c => compte.Courriel == c.Courriel);
                if (compteBd == null || compteBd.MotDePasse != compte.MotDePasse)
                {
                    ModelState.AddModelError("", "Le nom de compte ou le mot de passe est invalide");
                    return View(compte);
                }

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
        [HttpGet,Authorized]
        public ActionResult Deconnexion()
        {
            string cleSession = HttpContext.Request.Cookies["session"].Value;
            CacheSession.Instance.RetirerSession(Guid.Parse(cleSession));

            //Believe me or not, en changeant l'expiration du cookie c'est la seule façon de le supprimer du client
            HttpCookie cookie = new HttpCookie("session", "");
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.Cookies.Add(cookie);
            return View("Authentification");
        }

        /// <summary>
        /// C'est temporaire
        /// </summary>
        /// <returns></returns>
        [HttpGet,Authorized]
        public string text()
        { return "Yo dawg"; }
    }
}