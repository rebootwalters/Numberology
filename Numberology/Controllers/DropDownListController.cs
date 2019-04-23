using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology.Controllers
{
    public class DropDownListController : Controller
    {
        // GET: DropDownList
        public ActionResult Index()
        {
            using (BusinessLogicLayer.BLLContext ctx = new BusinessLogicLayer.BLLContext())
            {

                return View(ctx.RelatedNumbers.GetAllRelatedNumbers());
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (BusinessLogicLayer.BLLContext ctx = new BusinessLogicLayer.BLLContext())
            {
                var relatednumber = ctx.RelatedNumbers.FindRelatedNumber(id);
                var numbers = ctx.Numbers.GetAllNumbers();
                Models.DropDownViewForRelatedNumbers m = new Models.DropDownViewForRelatedNumbers();

                m.relatedNumber = relatednumber;
                m.SetValues(numbers);
                m.SelectedParentID = relatednumber.ParentNumberID;

                return View(m);
            }
        }

        [HttpPost]
        public ActionResult Edit(Models.DropDownViewForRelatedNumbers ddl)
        {
            int i = ddl.SelectedParentID;
            return View();
        }
    }
}