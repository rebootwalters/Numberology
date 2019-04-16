using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology.Controllers
{
    public class SharedController : Controller
    {
        [HttpGet]
        public ActionResult Unauthorized()
        {
            return View();
        }
        public ActionResult Unauthorized(string EMail, string Password, string ReturnURL)
        {
            using (BusinessLogicLayer.BLLContext ctx = new BusinessLogicLayer.BLLContext())
            {
                var user = ctx.Users.FindUserByEmail(EMail);
                if (user == null) return View((object)"Not a Valid EMail");
                string actual = user.Password;
                string potential = user.Salt + Password;
                bool checkedout = System.Web.Helpers.Crypto.
                    VerifyHashedPassword(actual,potential);
                if (checkedout)
                {
                    Session["AUTHUsername"] = EMail;
                    Session["AUTHRoles"] = user.Roles;
                    return Redirect(ReturnURL);
                }

                return View(View((object)"Not a Valid Login"));

           
            }
        }
    }
}