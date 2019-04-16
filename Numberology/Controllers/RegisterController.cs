using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace Numberology.Controllers
{
    public class RegisterController : Controller
    {



        string InvalidUser(BLLContext ctx, UserBLL user)
        {
            UserBLL test = ctx.Users.FindUserByEmail(user.EMailAddress);
            if (test!= null)
            {
                return $"A user with the EMail {user.EMailAddress} already exists.";
            }
            return null;
        }

        public ActionResult Verify(string ID, string Hash)
        {
            using (BLLContext ctx = new BLLContext())
            {
                int UID;
                bool found = int.TryParse(ID, out UID);
                UserBLL user = ctx.Users.FindByUserID(UID);
                if (user != null)
                {
                    if (user.Verified == "TRUE")
                    {
                        return View("Verified", (object)"User already Verified");
                    }
                     if (Hash == user.Verified)
                    {
                        user.Verified = "TRUE";
                        user.Comments = $"Verified on {DateTime.Now}";
                        ctx.Users.UpdateUser(user);
                        return View("Verified",(object)user.Comments);
                    }
                    else
                    {
                        View("Verified", (object)"Verification Code not valid");
                    }
                }
             
                return View("Verified", (object)"User not found");
                
            }
        }
        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View("Create");
        }
        [HttpPost]
        public ActionResult Index(UserBLL data)
        {
            using (BLLContext ctx = new BLLContext())
            {
                string message=message = InvalidUser(ctx,data);
                bool empty = string.IsNullOrEmpty(message);
                if (!empty)
                {
                    return View("InvalidUser", message);
                }
               
                string salt = System.Web.Helpers.Crypto.GenerateSalt(20);
                string verifyPhrase = System.Web.Helpers.Crypto.GenerateSalt(20);

                string hashedpassword = System.Web.Helpers.Crypto.HashPassword(salt + data.Password);

                data.Salt = salt;
                data.Password = hashedpassword;
                data.Verified = verifyPhrase;
                data.Roles = "Guest";
                data.Comments = "Un Verified";
                var newuser = ctx.Users.CreateUser(data);
                ViewBag.ID = newuser.UserID;
                ViewBag.Hash = data.Verified;
                return View("Created");
            }
        }
    }
}