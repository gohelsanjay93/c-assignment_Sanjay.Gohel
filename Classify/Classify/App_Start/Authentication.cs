using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Classify.App_Start
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;

            //if Session == null => Login page
            if (ctx.Session["UserId"] == null || ctx.Session.Keys.Equals("UserId"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "User" }));
            }
            base.OnActionExecuting(filterContext);
        }        
    }
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            //if Session == null => Login page
            if (ctx.Session["UserId"] != null || ctx.Session.Keys.Equals("UserId"))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}