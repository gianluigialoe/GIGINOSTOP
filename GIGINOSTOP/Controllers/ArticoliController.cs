using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace GIGINOSTOP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticoliController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            ViewBag.Piattaforme = new SelectList(db.Piattaforma.ToList(), "id", "nome");
            ViewBag.Categorie = new SelectList(db.Categoria.ToList(), "id", "nome");
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "nomearticolo, descrizione, idpiattaforma, idcategoria, prezzo, prezzo_offerta, in_offerta, idrecensione")] Articoli articolo, HttpPostedFileBase ImmagineFile, string ImmagineURL)
        {
            if (ModelState.IsValid)
            {
                // Se è stato fornito un URL per l'immagine, utilizzalo
                if (!string.IsNullOrEmpty(ImmagineURL))
                {
                    articolo.immagine = ImmagineURL;
                }
                else if (ImmagineFile != null && ImmagineFile.ContentLength > 0) // Altrimenti, controlla se è stato caricato un file
                {
                    // Salva l'immagine sul server
                    var fileName = Path.GetFileName(ImmagineFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Assets"), fileName);
                    ImmagineFile.SaveAs(path);

                    // Aggiorna il percorso dell'immagine nel modello
                    articolo.immagine = "~/Content/Assets/" + fileName;
                }

                // Aggiungi l'articolo al database
                db.Articoli.Add(articolo);
                db.SaveChanges();

                TempData["Conferma"] = true;
                return RedirectToAction("Lista");
            }

            // Se il modello non è valido, ricarica la vista con gli errori
            ViewBag.Piattaforme = new SelectList(db.Piattaforma.ToList(), "id", "nome", articolo.idpiattaforma);
            ViewBag.Categorie = new SelectList(db.Categoria.ToList(), "id", "nome", articolo.idcategoria);
            return View(articolo);
        }

        public ActionResult Lista()
        {
            var articoli = db.Articoli
                  .Include(a => a.Categoria)
                  .Include(a => a.Piattaforma)
                  .ToList();
            return View(articoli);
        }

        public ActionResult Dettagli(int id)
        {
            Articoli articolo = db.Articoli.Find(id);
            if (articolo == null)
            {
                return HttpNotFound();
            }
            // Nel metodo di azione in cui restituisci la vista:
            ViewBag.Piattaforme = new SelectList(db.Piattaforma, "id", "nome");
            ViewBag.Categorie = new SelectList(db.Categoria, "id", "nome");

            return View(articolo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modifica([Bind(Include = "id,nomearticolo,descrizione,immagine,idpiattaforma,idcategoria,prezzo,prezzo_offerta,in_offerta")] Articoli articolo, HttpPostedFileBase ImmagineFile, string ImmagineURL)
        {
            if (ModelState.IsValid)
            {
                var articoloEsistente = db.Articoli.Find(articolo.id);
                if (articoloEsistente != null)
                {
                    articoloEsistente.nomearticolo = articolo.nomearticolo;
                    articoloEsistente.descrizione = articolo.descrizione;
                    articoloEsistente.idpiattaforma = articolo.idpiattaforma;
                    articoloEsistente.idcategoria = articolo.idcategoria;
                    articoloEsistente.prezzo = articolo.prezzo;
                    articoloEsistente.in_offerta = articolo.in_offerta;

                    if (articolo.in_offerta == true)
                    {
                        articoloEsistente.prezzo_offerta = articolo.prezzo_offerta;
                    }
                    else
                    {
                        // Se l'articolo non è in offerta, impostiamo il prezzo offerta a null
                        articoloEsistente.prezzo_offerta = null;
                    }

                    // Controlla se è stato fornito un URL per l'immagine e aggiorna l'immagine di conseguenza
                    if (!string.IsNullOrEmpty(ImmagineURL))
                    {
                        articoloEsistente.immagine = ImmagineURL;
                    }
                    // Altrimenti, controlla se è stato fornito un file e aggiorna l'immagine di conseguenza
                    else if (ImmagineFile != null && ImmagineFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(ImmagineFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Assets"), fileName);
                        ImmagineFile.SaveAs(path);
                        articoloEsistente.immagine = "~/Content/Assets/" + fileName;
                    }

                    db.SaveChanges();
                    TempData["Conferma"] = true;
                }
                else
                {
                    TempData["Errore"] = "L'articolo specificato non esiste.";
                }
            }
            else
            {
                TempData["Errore"] = "Il modello non è valido.";
            }

            return RedirectToAction("Lista");
        }


        [HttpPost]
        public ActionResult Elimina(int ID)
        {
            var articoli = db.Articoli.Where(x => x.id == ID).FirstOrDefault();
            db.Articoli.Remove(articoli);
            db.SaveChanges();
            return RedirectToAction("Lista");

        }

    }


}



