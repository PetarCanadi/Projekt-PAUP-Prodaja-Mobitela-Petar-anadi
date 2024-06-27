using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    [Table("reklamacija")]
    public class Reklamacija
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ID mobitela je obavezan")]
        public int MobitelId { get; set; }

        [Required(ErrorMessage = "Proizvođač je obavezan")]
        public string Proizvodjac { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Cijena je obavezna")]
        public decimal Cijena { get; set; }

        [Required(ErrorMessage = "Datum kupnje je obavezan")]
        [Display(Name = "Datum Kupnje")]
        [DataType(DataType.Date)]
        public DateTime DatumKupnje { get; set; }

        [Required(ErrorMessage = "Opis kvara je obavezan")]
        [Display(Name = "Opis Kvara")]
        public string OpisKvara { get; set; }
    }
}
