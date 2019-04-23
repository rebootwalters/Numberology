using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace Numberology.Controllers
{
   
   [MustBeLoggedIn]
    public class AccountController : Controller
    {
        // GET: Account
     public ActionResult Index()
        {

            using (BLLContext ctx = new BLLContext())
            {
                var items = ctx.Users.GetAllUsers();
                return View(items);
            }
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Users.FindByUserID(id);
                return View(item);
            }
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(UserBLL user)
        {
            try
            {
                // TODO: Add insert logic here
                using (BLLContext ctx = new BLLContext())
                {
                    var item = ctx.Users.CreateUser(user);
                  
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Users.FindByUserID(id);
                Models.SelectedUserRolesModel m = new Models.SelectedUserRolesModel();
                m.User = item;
                m.SetSelectedValuesFromUser();          
                return View(m);
            }
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.SelectedUserRolesModel m)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    m.SetRolesFromSelectedValues();
                    ctx.Users.UpdateUser(m.User);
                    
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("exception",ex);
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Users.FindByUserID(id);
                return View(item);
            }
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Users.DeleteUser(id);
                   
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }
    }
}
