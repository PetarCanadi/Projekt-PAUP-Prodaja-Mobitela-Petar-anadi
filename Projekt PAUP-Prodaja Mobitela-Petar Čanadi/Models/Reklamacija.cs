using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class Reklamacija
    {
        public int Id { get; set; }
        public int MobitelId { get; set; }
        public string Proizvodjac { get; set; }
        public string Naziv { get; set; }
        public decimal Cijena { get; set; }
        public DateTime DatumKupnje { get; set; }
        public string OpisKvara { get; set; }
    }
}