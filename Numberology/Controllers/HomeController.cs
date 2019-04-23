using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Interesting Facts";

            return View();
        }

        public ActionResult Signin()
        {
            return View();
        }
        public ActionResult Signout()
        {
            Session.Remove("AUTHUsername");
            Session.Remove("AUTHRoles"); 
            return Redirect(@"\Home\");
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    // displays empty login screen
        //    return View();
        //}

        [HttpGet]
        public ActionResult Login()
        {
            Models.LoginModel m = new Models.LoginModel();
            m.Message = TempData["message"]?.ToString()??"x";
            m.ReturnURL = TempData["returnurl"]?.ToString()??@"~\Home";
            m.Username = "genericuser";
            m.Password = "genericpassword";
            return View(m);
        }
        [HttpPost]
        public ActionResult Login(Models.LoginModel logininfo)
        {
            // logic to authenticate user goes here
            using (BusinessLogicLayer.BLLContext ctx = new BusinessLogicLayer.BLLContext())
            {
                var user = ctx.Users.FindUserByEmail(logininfo.Username);
                if (user == null)
                
                    return View(logininfo.SetMessage("not a Valid Username"));
                
                string actual = user.Password;
                string potential = user.Salt + logininfo.Password;
                bool checkedout = System.Web.Helpers.Crypto.
                    VerifyHashedPassword(actual, potential);
                if (checkedout)
                {
                    Session["AUTHUsername"] = logininfo.Username;
                    Session["AUTHRoles"] = user.Roles;
                    if (logininfo.ReturnURL == null) logininfo.ReturnURL = @"~\home";  
                    return Redirect(logininfo.ReturnURL);
                }

                return View(logininfo.SetMessage("Not a Valid Login"));
                


            }
        }
    }
}