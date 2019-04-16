using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace Numberology.Controllers
{
    public class RiddleController : Controller
    {
        // GET: Riddle
        public ActionResult Index()
        {
            using (BLLContext ctx = new BLLContext())
            {
                var riddles = ctx.Riddles.GetAllRiddles();
                return View(riddles);
            }
        }

        public ActionResult Answer(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var r = ctx.Riddles.FindRiddle(id);
                if (null != r)
                {
                    return View(r);
                }
                else
                {
                    return View("MissingError", "The Specified Riddle does not exist");
                }
            }
        }

        // GET: Riddle/Details/5
        public ActionResult Details(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var r = ctx.Riddles.FindRiddle(id);
                if (null != r)
                {
                    return View(r);
                }
                else
                {
                    return View("MissingError", "The Specified Riddle does not exist");
                }
            }
        }

        // GET: Riddle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Riddle/Create
        [HttpPost]
        public ActionResult Create(RiddleBLL riddle)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Riddles.InsertNewRiddle(riddle);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception",ex);
            }
        }

        // GET: Riddle/Edit/5
        public ActionResult Edit(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var r = ctx.Riddles.FindRiddle(id);
                if (null != r)
                {
                    return View(r);
                }
                else
                {
                    return View("MissingError", "The Specified Riddle does not exist");

                }
            }
        }

        // POST: Riddle/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RiddleBLL riddle)
        {
            try
            {
                // TODO: Add update logic here
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Riddles.UpdateExistingRiddle(riddle);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception",ex);
            }
        }

        // GET: Riddle/Delete/5
        public ActionResult Delete(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var r = ctx.Riddles.FindRiddle(id);
                if (null != r)
                {
                    return View(r);
                }
                else
                {
                    return View("MissingError", "The Specified Riddle does not exist");

                }
            }
        }

        // POST: Riddle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, RiddleBLL riddle)
        {
            try
            {
                // TODO: Add update logic here
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.Riddles.DeleteExistingRiddle(riddle);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

    }
}
