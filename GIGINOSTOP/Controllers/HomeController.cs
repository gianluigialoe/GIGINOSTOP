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

        public ActionResult Dettagli()
        {
        
            return View();
        }
        public ActionResult Supermario()
        {

            return View();
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
        public ActionResult SearchArticles(string q)
        {
            // Confronta il termine di ricerca con il nomearticolo (ignorando maiuscole/minuscole)
            var results = db.Articoli
                            .Where(a => a.nomearticolo.ToLower().Contains(q.ToLower()))
                            .Select(a => new
                            {
                                a.id,
                                a.nomearticolo,
                                a.immagine,
                                a.prezzo,
                                a.prezzo_offerta
                            })
                            .ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}