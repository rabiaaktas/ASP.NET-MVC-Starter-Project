using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MStore.Components
{
    public class SessionAuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.HttpContext.Session["login"] == null || filterContext.HttpContext.Session["login"].ToString() != "ok")
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.HttpContext.Response.End();
            }
            
        }

        private bool skipAuthorization(AuthorizationContext filtercontext)
        {
            return filtercontext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }
    }
}