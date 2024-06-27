﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class RacunStavka
    {
        [Key]
        public int RacunStavkaID { get; set; }

        public int RacunID { get; set; }

        [ForeignKey("Mobitel")]
        public int MobitelID { get; set; } // Koristimo ID kao strani ključ

        public int Kolicina { get; set; }
        public decimal Cijena { get; set; }
        public decimal UkupnaCijena { get; set; }

        // Navigation properties
        [ForeignKey("RacunID")]
        public virtual Racun Racun { get; set; }
        [ForeignKey("MobitelID")]
        public virtual Mobitel Mobitel { get; set; }
    }
}
