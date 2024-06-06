using System;
using System.ComponentModel.DataAnnotations;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class RegistracijaKorisnika
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Unesite korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Unesite lozinku")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Unesite email adresu")]
        [EmailAddress(ErrorMessage = "Unesite valjanu email adresu")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Unesite svoje puno ime")]
        public string PrezimeIme { get; set; }
    }
}
