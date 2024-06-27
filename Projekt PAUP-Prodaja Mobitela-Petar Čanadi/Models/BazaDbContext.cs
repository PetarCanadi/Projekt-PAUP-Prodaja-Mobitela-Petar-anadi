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
        public DbSet<Reklamacija> Reklamacija { get; set; }
        public DbSet<Racun> Racuni { get; set; }
        public DbSet<RacunStavka> RacunStavke { get; set; }

        public BazaDbContext() : base("name=BazaDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mobitel>().ToTable("mobiteli");
            modelBuilder.Entity<RegistracijaKorisnika>().ToTable("registracijakorisnika");
            modelBuilder.Entity<PrijavaKorisnika>().ToTable("prijavakorisnika");
            modelBuilder.Entity<Racun>().ToTable("racuni");
            modelBuilder.Entity<Reklamacija>().ToTable("reklamacije");

            // Configure primary key for RacunStavka
            modelBuilder.Entity<RacunStavka>()
                   .HasKey(rs => rs.RacunStavkaID);

            // Configure primary key for Racun
            modelBuilder.Entity<Racun>()
                        .HasKey(r => r.RacunID);

            // Configure one-to-many relationship between Racun and RacunStavka
            modelBuilder.Entity<Racun>()
                        .HasMany(r => r.RacunStavke)
                        .WithRequired(rs => rs.Racun)
                        .HasForeignKey(rs => rs.RacunID);

            // Configure one-to-many relationship between Mobitel and RacunStavka
            modelBuilder.Entity<RacunStavka>()
                        .HasRequired(rs => rs.Mobitel)
                        .WithMany()
                        .HasForeignKey(rs => rs.MobitelID);
        }
    }
}
