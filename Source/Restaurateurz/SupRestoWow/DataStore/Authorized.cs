using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SupRestoWow.DataStore
{
    public class Authorized : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid cleSession = Guid.Parse(filterContext.HttpContext.Request.Cookies.Get("session").Value);
            var compte = CacheSession.Instance.ObtenirCompte(cleSession);
            if (compte == null)
            {
                throw new ApplicationException("T'es pas authentifié esti de chienne (oops).");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}