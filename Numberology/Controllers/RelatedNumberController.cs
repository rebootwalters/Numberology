using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogicLayer;

namespace Numberology.Controllers
{
  [MustBeInRole(Roles = "Contributer Administrator")]  public class RelatedNumberController : Controller
    {
        // GET: RelatedNumber
        public ActionResult Index()
        {
            using (BLLContext ctx = new BLLContext())
            {
                var list = ctx.RelatedNumbers.GetAllRelatedNumbers();

                return View(list);
            }
        }

        // GET: RelatedNumber/Details/5
        public ActionResult Details(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.RelatedNumbers.FindRelatedNumber(id);

                return View(item);
            }
        }

        // GET: RelatedNumber/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RelatedNumber/Create
        [HttpPost]
        public ActionResult Create(int ?returnTo ,RelatedNumberBLL relatednumber)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.RelatedNumbers.InsertNewRelatedNumber(relatednumber.RelatedName, relatednumber.RelatedLanguage, relatednumber.ParentNumberID);

                    
                }

                if (returnTo.HasValue & returnTo.Value >= 0)
                {
                    return RedirectToAction("ViewRelated", "Number", new { id = returnTo });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

        // GET: RelatedNumber/Edit/5
        public ActionResult Edit(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.RelatedNumbers.FindRelatedNumber(id);
                return View(item);
            }
        }

        // POST: RelatedNumber/Edit/5
        [HttpPost]
        public ActionResult Edit(int ?returnTo, int id, RelatedNumberBLL relatednumber)
        {
            try
            {
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.RelatedNumbers.PessimisticUpdateOfRelatedNumber( relatednumber);

                    if (returnTo.HasValue & returnTo.Value >= 0)
                    {
                        return RedirectToAction("ViewRelated", "Number", new { id = returnTo });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }

        // GET: RelatedNumber/Delete/5
        public ActionResult Delete(int id)
        {
            using (BLLContext ctx = new BLLContext())
            {
                var item = ctx.RelatedNumbers.FindRelatedNumber(id);
                return View(item);
            }
        }

        // POST: RelatedNumber/Delete/5
        [HttpPost]
        public ActionResult Delete(int? returnTo, int id, RelatedNumberBLL relatednumber)
        {
            try
            {
                // TODO: Add delete logic here
                using (BLLContext ctx = new BLLContext())
                {
                    ctx.RelatedNumbers.JustDeleteARelatedNumber(id);

                    if (returnTo.HasValue & returnTo.Value >= 0)
                    {
                        return RedirectToAction("ViewRelated", "Number", new { id = returnTo });
                    }
                    else
                    {
                        
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("exception", ex);
            }
        }
    }
}
