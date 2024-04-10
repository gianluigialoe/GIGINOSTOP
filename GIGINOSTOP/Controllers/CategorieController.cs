using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIGINOSTOP.Controllers
{

    [Authorize(Roles = "Admin")]

    public class CategorieController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            var categorie = db.Categoria.ToList(); return View(categorie);
        }

        public ActionResult Aggiungi()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Aggiungi([Bind(Include = "Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categoria.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoria);
        }
        public ActionResult Dettagli(int id)
        {
            var categoria = db.Categoria.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return View(categoria);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifica([Bind(Include = "ID, Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var categoriaEsistente = db.Categoria.Find(categoria.id);
                if (categoriaEsistente != null)
                {
                    categoriaEsistente.nome = categoria.nome;
                    db.SaveChanges();
                    TempData["Conferma"] = true;
                }
                else
                {
                    TempData["Errore"] = "La categoria specificata non esiste.";
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
            var categoria = db.Categoria.Where(x => x.id == ID).FirstOrDefault();
            if (categoria != null)
            {
                db.Categoria.Remove(categoria);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}