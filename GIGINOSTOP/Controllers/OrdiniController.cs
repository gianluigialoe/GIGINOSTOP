using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIGINOSTOP.Controllers
{
    public class OrdiniController : Controller
    {
        // GET: Ordini
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrdineConfermato()
        {
            return View();
        }

    }
}