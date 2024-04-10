using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIGINOSTOP.Models
{
    public class DettaglioArticoloViewModel
    {
        public Articoli Articolo { get; set; }
        public List<Recensioni> Recensioni { get; set; }
        public bool HaEffettuatoOrdine { get; set; }
        public int IdArticolo { get; set; }
        public string TestoRecensione { get; set; }
        public int PunteggioRecensione { get; set; }
    }
}