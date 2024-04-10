namespace GIGINOSTOP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recensioni")]
    public partial class Recensioni
    {
        public int id { get; set; }

        public int? idarticolo { get; set; }

        public int? idutente { get; set; }

        [StringLength(1000)]
        public string testo { get; set; }

        public int? punteggio { get; set; }

        public virtual Articoli Articoli { get; set; }

        public virtual Utenti Utenti { get; set; }
    }
}
