using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GIGINOSTOP.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
              : base("name=DBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<Articoli> Articoli { get; set; }
        public virtual DbSet<Carrello> Carrello { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<DettagliOrdine> DettagliOrdine { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Piattaforma> Piattaforma { get; set; }
        public virtual DbSet<Recensioni> Recensioni { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }
        public virtual DbSet<VociCarrello> VociCarrello { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articoli>()
                .Property(e => e.nomearticolo)
                .IsUnicode(false);

            modelBuilder.Entity<Articoli>()
                .Property(e => e.descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Articoli>()
                .Property(e => e.immagine)
                .IsUnicode(false);

            modelBuilder.Entity<Articoli>()
                .Property(e => e.prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Articoli>()
                .Property(e => e.prezzo_offerta)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Articoli>()
                .HasMany(e => e.DettagliOrdine)
                .WithOptional(e => e.Articoli)
                .HasForeignKey(e => e.idarticolo);

            modelBuilder.Entity<Articoli>()
                .HasMany(e => e.Recensioni)
                .WithOptional(e => e.Articoli)
                .HasForeignKey(e => e.idarticolo);

            modelBuilder.Entity<Articoli>()
                .HasMany(e => e.VociCarrello)
                .WithOptional(e => e.Articoli)
                .HasForeignKey(e => e.idarticolo);

            modelBuilder.Entity<Carrello>()
                .HasMany(e => e.VociCarrello)
                .WithOptional(e => e.Carrello)
                .HasForeignKey(e => e.idcarrello);

            modelBuilder.Entity<Categoria>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.Articoli)
                .WithOptional(e => e.Categoria)
                .HasForeignKey(e => e.idcategoria);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.DettagliOrdine)
                .WithOptional(e => e.Ordini)
                .HasForeignKey(e => e.idordine);

            modelBuilder.Entity<Piattaforma>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Piattaforma>()
                .HasMany(e => e.Articoli)
                .WithOptional(e => e.Piattaforma)
                .HasForeignKey(e => e.idpiattaforma);

            modelBuilder.Entity<Recensioni>()
                .Property(e => e.testo)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.ruolo)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.cognome)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Carrello)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.idutente);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Ordini)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.idutente);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Recensioni)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.idutente);

            modelBuilder.Entity<VociCarrello>()
                .Property(e => e.prezzo)
                .HasPrecision(10, 2);
        }
    }
}
