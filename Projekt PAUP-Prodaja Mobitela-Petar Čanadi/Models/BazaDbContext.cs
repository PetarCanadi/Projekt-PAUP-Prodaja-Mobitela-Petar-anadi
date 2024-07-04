using System.Data.Entity;
using MySql.Data.EntityFramework;
using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    // Koristi MySQL Entity Framework konfiguraciju
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        // DbSet-ovi za sve vaše entitete u bazi podataka
        public DbSet<Mobitel> Mobiteli { get; set; }
        public DbSet<PrijavaKorisnika> PrijavaKorisnika { get; set; }
        public DbSet<RegistracijaKorisnika> RegistracijaKorisnika { get; set; }
        public DbSet<Reklamacija> Reklamacije { get; set; } // DbSet za Reklamacije
        public DbSet<Racun> Racuni { get; set; }
        public DbSet<RacunStavka> RacunStavke { get; set; }

        // Konstruktor koji postavlja ime veze s bazom podataka
        public BazaDbContext() : base("name=BazaDbContext")
        {
        }

        // Metoda za konfiguraciju modela u bazi podataka
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapiranje svakog entiteta na odgovarajuću tablicu u bazi podataka
            modelBuilder.Entity<Mobitel>().ToTable("mobiteli");
            modelBuilder.Entity<RegistracijaKorisnika>().ToTable("registracijakorisnika");
            modelBuilder.Entity<PrijavaKorisnika>().ToTable("prijavakorisnika");
            modelBuilder.Entity<Racun>().ToTable("racuni");
            modelBuilder.Entity<Reklamacija>().ToTable("reklamacije"); // Mapiranje Reklamacija na "reklamacije" tablicu

            // Konfiguracija primarnog ključa za RacunStavka
            modelBuilder.Entity<RacunStavka>()
                .HasKey(rs => rs.RacunStavkaID);

            // Konfiguracija relacije jedan prema više između Racun i RacunStavka
            modelBuilder.Entity<Racun>()
                .HasKey(r => r.RacunID);

            modelBuilder.Entity<Racun>()
                .HasMany(r => r.RacunStavke)
                .WithRequired(rs => rs.Racun)
                .HasForeignKey(rs => rs.RacunID);

            // Konfiguracija relacije jedan prema jedan između Reklamacija i RacunStavka
            modelBuilder.Entity<Reklamacija>()
                .HasKey(r => r.ReklamacijaID);

            modelBuilder.Entity<Reklamacija>()
                .HasRequired(r => r.RacunStavka)
                .WithMany()
                .HasForeignKey(r => r.RacunStavkaID);
        }
    }
}
