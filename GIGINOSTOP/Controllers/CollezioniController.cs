using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GIGINOSTOP.Controllers
{

    public class CollezioniController : Controller
    {
        private DBContext db = new DBContext();
        // GET: Collezioni
        public ActionResult Index()
        {
            // Ottieni l'ID della categoria "console" (supponiamo che il nome sia esattamente "console")
            int? idCategoriaConsole = db.Categoria.FirstOrDefault(c => c.nome.ToLower() == "Console!")?.id;

            // Ottieni l'ID della piattaforma "PS5" (supponiamo che il nome sia esattamente "PS5")
            int? idPiattaformaPS5 = db.Piattaforma.FirstOrDefault(p => p.nome.ToLower() == "ps5")?.id;

            if (idCategoriaConsole != null && idPiattaformaPS5 != null && idCategoriaConsole != 0 && idPiattaformaPS5 != 0)
            {
                // Filtra e prendi al massimo 6 articoli che corrispondono alla categoria "console" e alla piattaforma "PS5"
                var articoliConsolePS5 = db.Articoli
                    .Where(a => a.idcategoria == idCategoriaConsole && a.idpiattaforma == idPiattaformaPS5)
                    .Take(6) // Limita a 6 articoli
                    .ToList();

                return View(articoliConsolePS5);
            }

            // Se non trovi la categoria "console" o la piattaforma "PS5", gestisci l'errore o restituisci una lista vuota
            return View(new List<Articoli>());
        }

        public ActionResult Details(int id)
        {
            var articolo = db.Articoli.Find(id);

            if (articolo == null)
            {
                return HttpNotFound();
            }

            var recensioni = db.Recensioni.Where(r => r.idarticolo == id).ToList();

            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            bool haEffettuatoOrdine = false;
            if (utente != null)
            {
                haEffettuatoOrdine = db.DettagliOrdine.Any(d => d.Ordini.idutente == utente.id && d.idarticolo == id);
            }

            var viewModel = new DettaglioArticoloViewModel
            {
                Articolo = articolo,
                Recensioni = recensioni,
                HaEffettuatoOrdine = haEffettuatoOrdine,
                IdArticolo = id
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ScriviRecensione(DettaglioArticoloViewModel viewModel)
        {
            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            if (utente == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bool haEffettuatoOrdine = db.DettagliOrdine.Any(d => d.Ordini.idutente == utente.id && d.idarticolo == viewModel.IdArticolo);

            if (!haEffettuatoOrdine)
            {
                return RedirectToAction("Details", new { id = viewModel.IdArticolo });
            }

            var recensione = new Recensioni
            {
                idarticolo = viewModel.IdArticolo,
                idutente = utente.id,
                testo = viewModel.TestoRecensione,
                punteggio = viewModel.PunteggioRecensione
            };

            db.Recensioni.Add(recensione);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = viewModel.IdArticolo });
        }
        [Authorize]
        [HttpPost]
        public ActionResult ModificaRecensione(int idRecensione, string nuovoTesto, int nuovoPunteggio)
        {
            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            if (utente == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var recensione = db.Recensioni.FirstOrDefault(r => r.id == idRecensione && r.idutente == utente.id);

            if (recensione == null)
            {
                return HttpNotFound(); // Recensione non trovata o non appartiene all'utente corrente
            }

            recensione.testo = nuovoTesto;
            recensione.punteggio = nuovoPunteggio;

            db.Entry(recensione).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = recensione.idarticolo });
        }

        [Authorize]
        [HttpPost]
        public ActionResult EliminaRecensione(int idRecensione)
        {
            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            if (utente == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var recensione = db.Recensioni.FirstOrDefault(r => r.id == idRecensione && r.idutente == utente.id);

            if (recensione == null)
            {
                return HttpNotFound(); // Recensione non trovata o non appartiene all'utente corrente
            }

            int idArticolo = (int)recensione.idarticolo;

            db.Recensioni.Remove(recensione);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = idArticolo });
        }
    }
}