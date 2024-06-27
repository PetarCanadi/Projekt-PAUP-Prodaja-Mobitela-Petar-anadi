using System.ComponentModel.DataAnnotations;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class PromjenaLozinkeModel
    {
        [Required(ErrorMessage = "Unesite korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Unesite staru lozinku")]
        [DataType(DataType.Password)]
        public string StaraLozinka { get; set; }

        [Required(ErrorMessage = "Unesite novu lozinku")]
        [DataType(DataType.Password)]
        public string NovaLozinka { get; set; }

        [Required(ErrorMessage = "Potvrdite novu lozinku")]
        [DataType(DataType.Password)]
        [Compare("NovaLozinka", ErrorMessage = "Nova lozinka i potvrda lozinke se ne podudaraju")]
        public string PotvrdaLozinke { get; set; }
    }
}
