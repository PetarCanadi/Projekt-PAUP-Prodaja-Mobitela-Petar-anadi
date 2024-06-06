using System.Data.Entity;
using MySql.Data.EntityFramework;
using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        public DbSet<Mobitel> Mobiteli { get; set; }
        public DbSet<PrijavaKorisnika> PrijavaKorisnika { get; set; }
        public DbSet<RegistracijaKorisnika> RegistracijaKorisnika { get; set; }
        public DbSet<Reklamacija> Reklamacije { get; set; }

        public BazaDbContext() : base("name=BazaDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mobitel>().ToTable("mobiteli");
            modelBuilder.Entity<Mobitel>().Property(m => m.ID).HasColumnName("ID");
            modelBuilder.Entity<Mobitel>().Property(m => m.Naziv).HasColumnName("Naziv");
            modelBuilder.Entity<Mobitel>().Property(m => m.Proizvodjac).HasColumnName("Proizvodjac");
            modelBuilder.Entity<Mobitel>().Property(m => m.Cijena).HasColumnName("Cijena");
            modelBuilder.Entity<Mobitel>().Property(m => m.Opis).HasColumnName("Opis");
            modelBuilder.Entity<Mobitel>().Property(m => m.GodinaModela).HasColumnName("GodinaModela");
        }
    }
}
