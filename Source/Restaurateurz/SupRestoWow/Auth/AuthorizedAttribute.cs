using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupRestoWow.Auth
{
    /// <summary>
    /// Vérifie si un utilisateur est authentifié
    /// </summary>
    public class AuthorizedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid cleSession = Guid.Parse(filterContext.HttpContext.Request.Cookies.Get("session").Value);
            var compte = CacheSession.Instance.ObtenirSession(cleSession);
            if (compte == null)
            {
                //Peut-être qu'il faudrait faire de quoi avec ça. lol
                throw new ApplicationException("T'es pas authentifié esti de chienne (oops).");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}