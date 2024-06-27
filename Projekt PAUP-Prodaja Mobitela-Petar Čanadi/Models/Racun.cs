using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class Racun
    {
        [Key]
        public int RacunID { get; set; }

        public DateTime DatumKupnje { get; set; }

        public string KorisnickoIme { get; set; }

        public decimal UkupanIznos { get; set; }

        // Navigation property for RacunStavka
        public virtual ICollection<RacunStavka> RacunStavke { get; set; }
    }
}
