using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Numberology.Controllers
{
    public class SimulateErrorController : Controller
    {
        // GET: SimulateError
        public ActionResult Index()
        {
            throw new Exception("This is the expected Exception");
        }
        public ActionResult E1()
        {
            throw new Exception("This is one expected Exception");
        }
    }
}