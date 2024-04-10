using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIGINOSTOP.Models
{
    public class CarrelloItem
    {
        public int ArticoloId { get; set; }
        public string NomeArticolo { get; set; }
        public decimal? Prezzo { get; set; }
        public int Quantita { get; set; } 
    }

}