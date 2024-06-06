using System.ComponentModel.DataAnnotations;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class AdminPrijava
    {
        [Required(ErrorMessage = "Unesite korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Unesite lozinku")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
    }
}
