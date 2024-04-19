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
        private List<Articoli> GetArticoliByCategoriaAndPiattaforma(string nomeCategoria, string nomePiattaforma, int takeCount = 6)
        {
            // Ottieni l'ID della categoria specificata
            int? idCategoria = db.Categoria.FirstOrDefault(c => c.nome.ToLower() == nomeCategoria.ToLower())?.id;

            // Ottieni l'ID della piattaforma specificata
            int? idPiattaforma = db.Piattaforma.FirstOrDefault(p => p.nome.ToLower() == nomePiattaforma.ToLower())?.id;

            if (idCategoria != null && idPiattaforma != null && idCategoria != 0 && idPiattaforma != 0)
            {
                // Filtra e prendi al massimo 'takeCount' articoli che corrispondono alla categoria e alla piattaforma specificate
                var articoli = db.Articoli
                                .Where(a => a.idcategoria == idCategoria && a.idpiattaforma == idPiattaforma)
                                .Take(takeCount)
                                .ToList();

                return articoli;
            }

            // Se la categoria o la piattaforma non sono trovate, restituisci una lista vuota
            return new List<Articoli>();
        }
        public ActionResult Index()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsolePS5 = GetArticoliByCategoriaAndPiattaforma("Console!", "PS5", 6);

            return View(articoliConsolePS5);
        }
        public ActionResult Xbox()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsoleXbox = GetArticoliByCategoriaAndPiattaforma("Console!", "Xbox", 6);

            return View(articoliConsoleXbox);
        }
        public ActionResult Nindendo()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsoleNindendo = GetArticoliByCategoriaAndPiattaforma("Console!", "Nindendo", 6);

            return View(articoliConsoleNindendo);
        }
        public ActionResult Bandai()
        {
         

            return View();
        }
        public ActionResult PC()
        {


            return View();
        }
        public ActionResult Very()
        {


            return View();
        }
        public ActionResult Hori()
        {


            return View();
        }
        public ActionResult Fc24()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsoleFc24 = GetArticoliByCategoriaAndPiattaforma("Fc24", "Fc24", 4);

            return View(articoliConsoleFc24);
        }
        public ActionResult Pokemon()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsolePokemon = GetArticoliByCategoriaAndPiattaforma("Pokemon", "Pokemon", 6);

            return View(articoliConsolePokemon);
        }
        public ActionResult Streamer()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsoleStreamer = GetArticoliByCategoriaAndPiattaforma("Streamer", "Streamer", 6);

            return View(articoliConsoleStreamer);
        }
        public ActionResult Msi()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsoleMsi = GetArticoliByCategoriaAndPiattaforma("Msi", "Msi", 6);

            return View(articoliConsoleMsi);
        }
        public ActionResult Ps1()
        {
            // Ottieni gli articoli per la collezione PlayStation (categoria: "Console!", piattaforma: "PS5")
            var articoliConsolePs1 = GetArticoliByCategoriaAndPiattaforma("Console!", "Ps1", 6);

            return View(articoliConsolePs1);
        }


        public ActionResult Details(int id)
        {
            var articolo = db.Articoli.Find(id);

            if (articolo == null)
            {
                return HttpNotFound();
            }

            // Carica le recensioni con l'oggetto Utente associato
            var recensioni = db.Recensioni
                                .Include(r => r.Utenti)
                                .Where(r => r.idarticolo == id)
                                .ToList();

            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            bool haEffettuatoOrdine = false;
            if (utente != null)
            {
                haEffettuatoOrdine = db.DettagliOrdine.Any(d => d.Ordini.idutente == utente.id && d.idarticolo == id);
            }

            // Trova articoli correlati basati sulla stessa categoria dell'articolo corrente
            var articoliCorrelati = db.Articoli
                .Where(a => a.id != id && a.idcategoria == articolo.idcategoria && a.idpiattaforma ==articolo.idpiattaforma)
                .Take(6) // Limita a 3 articoli correlati
                .ToList();

            var viewModel = new DettaglioArticoloViewModel
            {
                Articolo = articolo,
                Recensioni = recensioni,
                HaEffettuatoOrdine = haEffettuatoOrdine,
                IdArticolo = id,
                ArticoliCorrelati = articoliCorrelati
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
        public ActionResult EliminaRecensione(int recensioneId)
        {
            var recensione = db.Recensioni.Find(recensioneId);

            if (recensione == null)
            {
                return HttpNotFound();
            }

            // Verifica se l'utente autenticato è l'autore della recensione
            if (recensione.idutente.ToString() == User.Identity.Name)
            {
                db.Recensioni.Remove(recensione);
                db.SaveChanges();

                return RedirectToAction("Details", "Collezioni", new { id = recensione.idarticolo });
            }

            // Se l'utente non è autorizzato a eliminare questa recensione, gestisci l'errore
            return RedirectToAction("Index", "Home"); // Redirezione a una pagina di errore o home
        }

    }
}
