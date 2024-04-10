namespace GIGINOSTOP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Articoli")]
    public partial class Articoli
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articoli()
        {
            DettagliOrdine = new HashSet<DettagliOrdine>();
            Recensioni = new HashSet<Recensioni>();
            VociCarrello = new HashSet<VociCarrello>();
        }

        public int id { get; set; }

        [StringLength(255)]
        public string nomearticolo { get; set; }

        [StringLength(1000)]
        public string descrizione { get; set; }

        [StringLength(255)]
        public string immagine { get; set; }

        public int? idpiattaforma { get; set; }

        public int? idcategoria { get; set; }

        public decimal? prezzo { get; set; }

        public decimal? prezzo_offerta { get; set; }

        public bool? in_offerta { get; set; }

        public int? idrecensione { get; set; }

        public virtual Categoria Categoria { get; set; }

        public virtual Piattaforma Piattaforma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettagliOrdine> DettagliOrdine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensioni> Recensioni { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VociCarrello> VociCarrello { get; set; }
    }
}
