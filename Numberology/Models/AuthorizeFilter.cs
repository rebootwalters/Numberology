using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
           // check if user is indeed in the role.  
            if (this.Roles.Split(' ').Any(filterContext.HttpContext.User.IsInRole))
            {
                //Call base class to allow user into the action method
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                // user is not in the required role.  Redirect to a login page
                var vr = new ViewResult();
                vr.ViewName = "~/views/Shared/Unauthorized.cshtml";
                vr.ViewData["Roles"] = Roles;
                vr.ViewData["ReturnURL"] = filterContext.RequestContext.HttpContext.Request.Path.ToString();





                filterContext.Result = vr;

            }
        }
    }
}