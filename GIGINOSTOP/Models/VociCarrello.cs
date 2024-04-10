namespace GIGINOSTOP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VociCarrello")]
    public partial class VociCarrello
    {
        public int id { get; set; }

        public int? idcarrello { get; set; }

        public int? idarticolo { get; set; }

        public int? quantita { get; set; }

        public decimal? prezzo { get; set; }

        public virtual Articoli Articoli { get; set; }

        public virtual Carrello Carrello { get; set; }
    }
}
