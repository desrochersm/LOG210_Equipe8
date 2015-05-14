using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupRestoWow.Models;

namespace SupRestoWow.Auth
{
    /// <summary>
    /// Cache statique de session
    /// </summary>
    internal class CacheSession
    {
        private static readonly object @lock = new object();
        private static CacheSession cacheSession;

        internal Dictionary<Guid,Compte> comptesUtilisateursParCle;

        /// <summary>
        /// Instance de cache
        /// </summary>
        internal static CacheSession Instance 
        {
            get
            {
                if(cacheSession == null)
                {
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
            comptesUtilisateursParCle = new Dictionary<Guid,Compte>();
        }
            
        /// <summary>
        /// Ajouter un compte en session
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        internal Guid AjouterSession(Compte compte)
        {
            Guid cleSession = Guid.NewGuid();
            comptesUtilisateursParCle.Add(cleSession, compte);
            return cleSession;
        }

        /// <summary>
        /// Obtenir un compte en session
        /// </summary>
        /// <param name="cleSession"></param>
        /// <returns></returns>
        internal Compte ObtenirSession(Guid cleSession)
        {
            Compte compte;
            bool compteExiste = comptesUtilisateursParCle.TryGetValue(cleSession, out compte);

            return compteExiste ? compte : null;
        }

        /// <summary>
        /// Retirer une session du cache
        /// </summary>
        /// <param name="cleSession"></param>
        internal void RetirerSession(Guid cleSession)
        {
            comptesUtilisateursParCle.Remove(cleSession);
        }
    }
}