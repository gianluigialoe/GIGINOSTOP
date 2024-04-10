using GIGINOSTOP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace GIGINOSTOP.Controllers
{
    public class CarrelloController : Controller
    {
        DBContext db = new DBContext();

  

            // Azione per visualizzare il carrello
            public ActionResult Visualizza()
            {
                List<CarrelloItem> carrello;

                if (User.Identity.IsAuthenticated)
                {
                    // Recupera il carrello dal database per l'utente autenticato
                    var userId = User.Identity.Name;
                var user = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);
                    if (user != null)
                    {
                        var userIdInt = user.id;

                        carrello = db.VociCarrello
                            .Where(vc => vc.Carrello.idutente == userIdInt)
                            .Select(vc => new CarrelloItem
                            {
                                ArticoloId = (int)vc.idarticolo,
                                NomeArticolo = vc.Articoli.nomearticolo,
                                Prezzo = vc.prezzo,
                                Quantita = (int)vc.quantita
                            })
                            .ToList();
                    }
                    else
                    {
                        carrello = new List<CarrelloItem>();
                    }
                }
                else
                {
                    // Recupera il carrello dal cookie per gli utenti non autenticati
                    var carrelloJson = Request.Cookies["Carrello"];
                    carrello = carrelloJson != null ?
                        JsonConvert.DeserializeObject<List<CarrelloItem>>(carrelloJson.Value) :
                        new List<CarrelloItem>();
                }

                return View(carrello);
            }

        [HttpPost]
        public ActionResult AggiungiAlCarrello(int id, int quantita)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.Name;
                var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

                if (utente != null)
                {
                    var voceCarrello = db.VociCarrello.FirstOrDefault(vc => vc.idarticolo == id && vc.Carrello.idutente == utente.id);

                    if (voceCarrello != null)
                    {
                        voceCarrello.quantita += quantita;
                    }
                    else
                    {
                        var carrello = db.Carrello.FirstOrDefault(c => c.idutente == utente.id);
                        if (carrello == null)
                        {
                            carrello = new Carrello { idutente = utente.id };
                            db.Carrello.Add(carrello);
                        }

                        db.VociCarrello.Add(new VociCarrello
                        {
                            idcarrello = carrello.id,
                            idarticolo = id,
                            quantita = quantita,
                            prezzo = db.Articoli.Find(id).prezzo
                        });
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            else
            {
                var carrelloJson = Request.Cookies["Carrello"];
                var carrello = carrelloJson != null ?
                    JsonConvert.DeserializeObject<List<CarrelloItem>>(carrelloJson.Value) :
                    new List<CarrelloItem>();

                var articolo = carrello.FirstOrDefault(item => item.ArticoloId == id);
                if (articolo != null)
                {
                    articolo.Quantita += quantita;
                }
                else
                {
                    articolo = new CarrelloItem
                    {
                        ArticoloId = id,
                        NomeArticolo = db.Articoli.Find(id).nomearticolo,
                        Prezzo = db.Articoli.Find(id).prezzo,
                        Quantita = quantita
                    };
                    carrello.Add(articolo);
                }

                var carrelloJsonString = JsonConvert.SerializeObject(carrello);
                Response.Cookies.Add(new HttpCookie("Carrello", carrelloJsonString));

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        // Azione per eliminare un articolo dal carrello
        [HttpPost]
            public ActionResult EliminaArticolo(int id)
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Recupera l'utente autenticato
                    var userId = User.Identity.Name;
                var user = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);
                    if (user != null)
                    {
                        // Rimuovi l'articolo dal carrello nel database
                        var userIdInt = user.id;
                        var voceCarrello = db.VociCarrello.FirstOrDefault(vc => vc.idarticolo == id && vc.Carrello.idutente == userIdInt);
                        if (voceCarrello != null)
                        {
                            db.VociCarrello.Remove(voceCarrello);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    // Recupera il carrello dal cookie per gli utenti non autenticati
                    var carrelloJson = Request.Cookies["Carrello"];
                    var carrello = carrelloJson != null ?
                        JsonConvert.DeserializeObject<List<CarrelloItem>>(carrelloJson.Value) :
                        new List<CarrelloItem>();

                    // Trova e rimuovi l'articolo dal carrello
                    var articolo = carrello.FirstOrDefault(item => item.ArticoloId == id);
                    if (articolo != null)
                    {
                        carrello.Remove(articolo);

                        // Serializza il carrello in JSON
                        var carrelloJsonString = JsonConvert.SerializeObject(carrello);

                        // Aggiorna il cookie con il carrello modificato
                        Response.Cookies.Add(new HttpCookie("Carrello", carrelloJsonString));
                    return Json(new { success = true });

                }
                }

                // Redirect alla pagina del carrello
                return RedirectToAction("Visualizza");
            }

            // Azione per aggiornare la quantità di un articolo nel carrello
            [HttpPost]
            public ActionResult AggiornaQuantita(int id, int quantita)
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Recupera l'utente autenticato
                    var userId = User.Identity.Name;
                var user = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);
                    if (user != null)
                    {
                        // Aggiorna la quantità dell'articolo nel carrello nel database
                        var userIdInt = user.id;
                        var voceCarrello = db.VociCarrello.FirstOrDefault(vc => vc.idarticolo == id && vc.Carrello.idutente == userIdInt);
                        if (voceCarrello != null)
                        {
                            voceCarrello.quantita = quantita;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    // Recupera il carrello dal cookie per gli utenti non autenticati
                    var carrelloJson = Request.Cookies["Carrello"];
                    var carrello = carrelloJson != null ?
                        JsonConvert.DeserializeObject<List<CarrelloItem>>(carrelloJson.Value) :
                        new List<CarrelloItem>();

                    // Trova l'articolo nel carrello
                    var articolo = carrello.FirstOrDefault(item => item.ArticoloId == id);
                    if (articolo != null)
                    {
                        // Modifica la quantità dell'articolo
                        articolo.Quantita = quantita;

                        // Serializza il carrello in JSON
                        var carrelloJsonString = JsonConvert.SerializeObject(carrello);

                        // Aggiorna il cookie con il carrello modificato
                        Response.Cookies.Add(new HttpCookie("Carrello", carrelloJsonString));
                    }
                }

                // Redirect alla pagina del carrello
                return RedirectToAction("Visualizza");
            }

        [Authorize]
        [HttpPost]
        public ActionResult ConfermaOrdine()
        {
            var userId = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.id.ToString() == userId);

            if (utente != null)
            {
                var carrello = db.Carrello.Include(c => c.VociCarrello).FirstOrDefault(c => c.idutente == utente.id);

                if (carrello != null && carrello.VociCarrello.Any())
                {
                    // Creazione dell'ordine
                    var nuovoOrdine = new Ordini
                    {
                        idutente = utente.id,
                        data_ordine = DateTime.Now
                    };

                    db.Ordini.Add(nuovoOrdine);

                    foreach (var voceCarrello in carrello.VociCarrello)
                    {
                        var dettaglioOrdine = new DettagliOrdine
                        {
                            idordine = nuovoOrdine.id,
                            idarticolo = voceCarrello.idarticolo,
                            quantita = voceCarrello.quantita
                        };

                        db.DettagliOrdine.Add(dettaglioOrdine);
                    }

                    // Rimuovi il carrello una volta creato l'ordine
                    db.Carrello.Remove(carrello);

                    db.SaveChanges();

                    return RedirectToAction("OrdineConfermato","Ordini");
                }
            }

            return RedirectToAction("Index", "Home"); // Ritorna alla home in caso di errore
        }
    }
    }