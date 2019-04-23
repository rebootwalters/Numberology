using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology.Controllers
{
   [MustBeInRole(Roles = "User Contributer Administrator")] public class NumberController : Controller
    {

        public ActionResult ViewRelated(int id)
        {
            
            using (BLLContext ctx = new BLLContext())
            {
                NumberBLL item = ctx.Numbers.FindNumber(id);
                ctx.LoadingItems.LoadRelatedNumbersIntoNumber(item);
            
                return View(item);
            }
        }
        // GET: Number
        public ActionResult Index()
        {
            using (BLLContext ctx = new BLLContext())
            {
                
                var list = ctx.Numbers.GetAllNumbers();

                return View(list);
            }
        }

        // GET: Number/Details/5
        public ActionResult Details(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Numbers.FindNumber(id);
                return View(item);
            }
        }

        // GET: Number/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Number/Create
        [HttpPost]
        public ActionResult Create(NumberBLL number)
        {
            try
            {
                
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Numbers.InsertNewNumber(number.Name, number.Doublestuff, number.Floatstuff);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

        // GET: Number/Edit/5
        public ActionResult Edit(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Numbers.FindNumber(id);
                return View(item);
            }
        }

        // POST: Number/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BusinessLogicLayer.NumberBLL number)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Numbers.PessimisticUpdateOfNumber(id, number);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

        // GET: Number/Delete/5
        public ActionResult Delete(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.Numbers.FindNumber(id);
                return View(item);
            }
        }

        // POST: Number/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, NumberBLL number)
        {
            try
            {
                // TODO: Add delete logic here
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Numbers.JustDeleteANumber(id);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception",ex);
            }
        }
    }
}
