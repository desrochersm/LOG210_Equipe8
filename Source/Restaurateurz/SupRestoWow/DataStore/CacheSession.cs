using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SupRestoWow.Models;

namespace SupRestoWow.DataStore
{
    internal class CacheSession
    {
        private static readonly object @lock = new object();
        private static CacheSession cacheSession;

        internal Dictionary<Guid,Compte> _comptesUtilisateursParCle;

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

        internal CacheSession()
        {
            _comptesUtilisateursParCle = new Dictionary<Guid,Compte>();
        }
            
        internal Guid AjouterCompte(Compte compte)
        {
            Guid cleSession = Guid.NewGuid();
            _comptesUtilisateursParCle.Add(cleSession, compte);
            return cleSession;
        }

        internal Compte ObtenirCompte(Guid cleSession)
        {
            Compte compte;
            bool compteExiste = _comptesUtilisateursParCle.TryGetValue(cleSession, out compte);

            return compteExiste ? compte : null;
        }
    }
}