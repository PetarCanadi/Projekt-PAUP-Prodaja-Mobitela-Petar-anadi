using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class Reklamacija
    {
        public int ReklamacijaID { get; set; }

        public string Opis { get; set; }

        public DateTime DatumReklamacije { get; set; }

        public int RacunStavkaID { get; set; }

        [NotMapped]
        public string NazivMobitela { get; set; }

        public virtual RacunStavka RacunStavka { get; set; }

        // Svojstvo za korisničko ime korisnika koji je prijavio reklamaciju
        public string KorisnickoIme { get; set; }
    }
}
