namespace GIGINOSTOP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DettagliOrdine")]
    public partial class DettagliOrdine
    {
        public int id { get; set; }

        public int? idordine { get; set; }

        public int? idarticolo { get; set; }

        public int? quantita { get; set; }

        public virtual Articoli Articoli { get; set; }

        public virtual Ordini Ordini { get; set; }
    }
}
