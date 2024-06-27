using System.ComponentModel.DataAnnotations;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class AdminPrijava
    {
        [Required(ErrorMessage = "Molimo unesite korisničko ime")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Molimo unesite lozinku")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
    }
}
