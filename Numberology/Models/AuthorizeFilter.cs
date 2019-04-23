using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology
{
    public class MustBeInRoleAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // check if user is indeed in the role.  
            if (this.Roles.Split(' ').Any(filterContext.HttpContext.User.IsInRole))
            {
                //Call base class to allow user into the action method
                base.OnAuthorization(filterContext); ;
            }
            else
            {
                // user is not in the required role.  Redirect to a login page
                string ReturnURL = filterContext.RequestContext.HttpContext.Request.Path.ToString();

                filterContext.Controller.TempData.Add("Message",
        $"you must be in at least one of the following roles to access this resource:  {this.Roles}");
                filterContext.Controller.TempData.Add("ReturnURL", ReturnURL);
                System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
                dict.Add("Controller", "Home");
                dict.Add("Action", "Login");
                filterContext.Result = new RedirectToRouteResult(dict);


            }
        }
    }

    public class MustBeLoggedInAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // check if user is indeed in the role.  
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //Call base class to allow user into the action method
                base.OnAuthorization(filterContext);
            }
            else
            {
                // user is not Logged in.  Redirect to a login page
                string ReturnURL = filterContext.RequestContext.HttpContext.Request.Path.ToString();

                filterContext.Controller.TempData.Add("Message",
        $"you must be logged into some account to access this resource.  You are not logged in");
                filterContext.Controller.TempData.Add("ReturnURL", ReturnURL);
                System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
                dict.Add("Controller", "Home");
                dict.Add("Action", "Login");
                filterContext.Result = new RedirectToRouteResult(dict);


            }
        }
    }
}
