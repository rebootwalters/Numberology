
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Helpers;

namespace Numberology.Controllers
{
    public class HashController : Controller
    {
        public ActionResult View(string username, string password)
        {
            ViewBag.salted = "";
            ViewBag.Title = "VIEW";
            ViewBag.Message = $"Here is the Session";
            return View("details");
        }

        public ActionResult Register(string username, string password)
        {
            string salt = System.Web.Helpers.Crypto.GenerateSalt(0);
            HttpContext.Session["salt:"+username] = salt;
            string saltedpassword = salt + password;
            ViewBag.salted = saltedpassword;
            string hash = System.Web.Helpers.Crypto.HashPassword(saltedpassword);
            


            HttpContext.Session["hash:" + username] = hash;
            ViewBag.Title = "REGISTER";
            ViewBag.Message = $"Registerd User {username} with Hash {hash}";
            return View("details");
            
        }
        public ActionResult Login(string username, string password)
        {
            string salt = HttpContext.Session["salt:" + username]as string;
            if (salt == null)
            {
                ViewBag.Title = "LOGIN";
                ViewBag.Message = "username not found";
                return (View("details"));
            }
            string saltedpassword = salt + password;
            ViewBag.salted = saltedpassword;
            
            string actualhash = HttpContext.Session["hash:" + username] as string;
            if (actualhash == null)
            {
                ViewBag.Title = "LOGIN";
                ViewBag.Message = "username found but hash not found";
                return (View("details"));

            }
            if (System.Web.Helpers.Crypto.VerifyHashedPassword(actualhash,saltedpassword))
            {
                ViewBag.Title = "LOGIN";
                ViewBag.Message = "Valid LOGIN";
                return (View("details"));

            }
            else
            {
                ViewBag.Title = "LOGIN";
                ViewBag.Message = $"INValid LOGIN actual={actualhash}";
                return (View("details"));
            }

        }
        // GET: Hash
        public ActionResult Forms(string id)
        {
            string x = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(id, "MD5");
            ViewBag.id= id;
            ViewBag.pw = x;
            return View("Details");
        }

        public ActionResult Crypto(string id)
        {
            string x = System.Web.Helpers.Crypto.HashPassword(id);
            string s = System.Web.Helpers.Crypto.GenerateSalt(16);
            string sp = System.Web.Helpers.Crypto.HashPassword(s + id);
            ViewBag.id = id;
            ViewBag.pw = x;
            ViewBag.salt = s;
            ViewBag.pwwithsalt = s + id;
            ViewBag.saltedpw = sp;
            return View("Details");
            
        }
    }
}