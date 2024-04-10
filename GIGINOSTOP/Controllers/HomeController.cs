using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIGINOSTOP.Controllers
{
    public class HomeController : Controller
    {
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            List<Articoli> articoli = db.Articoli.ToList(); // Supponendo che il tuo modello abbia una classe chiamata Articolo

            return View(articoli);
        }

        public ActionResult Dettagli(int id)
        {
            Articoli articolo = db.Articoli.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }


            return View(articolo);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}