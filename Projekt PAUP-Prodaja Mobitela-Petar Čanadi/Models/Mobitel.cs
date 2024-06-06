using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    [Table("mobiteli")]
    public class Mobitel
    {
        [Key]
        [Display(Name = "ID mobitela")]
        public int ID { get; set; }

        [Display(Name = "Naziv")]
        [Required(ErrorMessage = "{0} je obavezno")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} mora biti minimalno 2 znaka")]
        public string Naziv { get; set; }

        [Display(Name = "Proizvođač")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} mora biti minimalno 2 znaka")]
        public string Proizvodjac { get; set; }

        public string ProizvodjacNaziv
        {
            get
            {
                return Proizvodjac + " " + Naziv;
            }
        }

        [Display(Name = "Cijena")]
        public decimal? Cijena { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} mora biti minimalno 2 znaka")]
        public string Opis { get; set; }

        [Display(Name = "Godina modela")]
        [Column("GodinaModela")]
        [Range(2022, 2024, ErrorMessage = "Vrijednost mora biti između 2022 do 2024")]
        public GodinaModela GodinaModela { get; set; }
    }
}
