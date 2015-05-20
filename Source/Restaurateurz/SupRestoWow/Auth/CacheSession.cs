using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupRestoWow.Models;
using System.Collections.Concurrent;

namespace SupRestoWow.Auth
{
    /// <summary>
    /// Cache statique de session
    /// </summary>
    public class CacheSession
    {
        private static readonly object @lock = new object();
        private static CacheSession cacheSession;

        private readonly ConcurrentDictionary<Guid,Compte> comptesUtilisateursParCle;

        /// <summary>
        /// Instance de cache
        /// </summary>
        public static CacheSession Instance 
        {
            get
            {
                if(cacheSession == null)
                {
                    //Afin de ne pas instancier le cache deux fois, ça pourrait être problématique.
                    lock(@lock)
                    {
                        if(cacheSession == null)
                            cacheSession = new CacheSession();
                    }
                }

                return cacheSession;
            }
        }

        private CacheSession()
        {
            comptesUtilisateursParCle = new ConcurrentDictionary<Guid,Compte>();
        }
            
        /// <summary>
        /// Ajouter un compte en session
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        public Guid AjouterSession(Compte compte)
        {
            Guid cleSession = Guid.NewGuid();
            
            bool sessionAjoutee = comptesUtilisateursParCle.TryAdd(cleSession, compte);

            if(!sessionAjoutee)
            {
                //Ça ne devrait pas arrivé étant donner que c'est un petit labo..
                throw new ApplicationException("Erreur de concurrence de connexion");
            }
            
            return cleSession;
        }

        /// <summary>
        /// Obtenir un compte en session
        /// </summary>
        /// <param name="cleSession"></param>
        /// <returns></returns>
        public Compte ObtenirSession(Guid cleSession)
        {
            Compte compte;
            bool compteExiste = comptesUtilisateursParCle.TryGetValue(cleSession, out compte);

            return compteExiste ? compte : null;
        }

        /// <summary>
        /// Retirer une session du cache
        /// </summary>
        /// <param name="cleSession"></param>
        public void RetirerSession(Guid cleSession)
        {
            Compte compte = null;
            comptesUtilisateursParCle.TryRemove(cleSession, out compte);
        }
    }
}