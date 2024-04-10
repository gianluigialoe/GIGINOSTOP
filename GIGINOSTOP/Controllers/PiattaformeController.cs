using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIGINOSTOP.Controllers
{
    [Authorize(Roles = "Admin")]

    public class PiattaformeController : Controller
    {
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            var piattaforme = db.Piattaforma.ToList(); 
            return View(piattaforme);
        }

        public ActionResult Aggiungi()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aggiungi([Bind(Include = "Nome")] Piattaforma piattaforma)
        {
            if (ModelState.IsValid)
            {
                db.Piattaforma.Add(piattaforma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(piattaforma);
        }
        public ActionResult Dettagli(int id)
        {
            var piattaforma = db.Piattaforma.Find(id);
            if (piattaforma == null)
            {
                return HttpNotFound();
            }
            return View(piattaforma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifica([Bind(Include = "ID, Nome")] Piattaforma piattaforma)
        {
            if (ModelState.IsValid)
            {
                var piattaformaEsistente = db.Piattaforma.Find(piattaforma.id);
                if (piattaformaEsistente != null)
                {
                    piattaformaEsistente.nome = piattaforma.nome;
                    db.SaveChanges();
                    TempData["Conferma"] = true;
                }
                else
                {
                    TempData["Errore"] = "La piattaforma specificata non esiste.";
                }
            }
            else
            {
                TempData["Errore"] = "Il modello non è valido.";
            }
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult Elimina(int ID)
        {
            var Piattaforma = db.Piattaforma.Where(x => x.id == ID).FirstOrDefault();
            db.Piattaforma.Remove(Piattaforma);
            db.SaveChanges();
            return RedirectToAction("Index");

        }



    }
}
