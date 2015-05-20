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
        public ActionResult Creer([Bind(Exclude = "Id")]Compte compte) //Todo: Voir s'il n'aurait pas une meilleure faire!
        {
            if (!ModelState.IsValid) 
                return View("CreationCompte", compte);
                
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
            return View("ConfirmationCompte", compte);
        }

        /// <summary>
        /// Afficher la vue pour modifier un compte
        /// </summary>
        /// <returns></returns>
        [Authorized]
        public ActionResult Modifier()
        {
            //Afficher la page avec les informations du compte
            return View(ObtenirCompteCourant());
        }

        /// <summary>
        /// Modifier un compte
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        [HttpPost,Authorized]
        public ActionResult Modifier(Compte compte)
        {
            if (!ModelState.IsValid)
                return View("Modifier", compte);

            Compte compteCourant = ObtenirCompteCourant();
            //Va lancer une exception jusqu'à ce que l'on ait une BD

            using (RestaurentContext context = new RestaurentContext())
            {
                var compteBd = context.Set<Compte>().SingleOrDefault(c => c.Id == compteCourant.Id);

                if (compteBd == null)
                {
                    throw new ApplicationException("Le compte n'a pas été trouvé dans le database");
                }

                //Temporaire
                compteBd.DateNaissance = compte.DateNaissance;
                compteBd.Adresse = compte.Adresse;
                compteBd.Courriel = compte.Courriel;
                compteBd.MotDePasse = compte.MotDePasse;
                compteBd.Nom = compte.Nom;
                compteBd.Telephone = compte.Telephone;
                
                //context.Entry(compteCourant).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            //On retourne où maintenant?
            return View("ConfirmationCompte", compte);
        }

        /// <summary>
        /// Obtenir le compte courant qui est dans le cache
        /// </summary>
        /// <returns></returns>
        private Compte ObtenirCompteCourant()
        { 
            return CacheSession.Instance.ObtenirSession(Guid.Parse(this.HttpContext.Request.Cookies["session"].Value));
        }

        /// <summary>
        /// Obtention de la vue d'authentification
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Connexion()
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

                Guid cleSession = CacheSession.Instance.AjouterSession(compteBd);
                HttpContext.Response.Cookies.Add(new HttpCookie("session", cleSession.ToString()));

                //Ajouter notre de où on s'en va!
                return RedirectToAction("Index","Home");//Redirect("");
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
            return View("Connexion");
        }

        /// <summary>
        /// C'est temporaire
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string text()
        {
            RestaurentContext c = new RestaurentContext();
            bool asd = c.Database.Exists();
            return "Yo dawg"; }
    }
}