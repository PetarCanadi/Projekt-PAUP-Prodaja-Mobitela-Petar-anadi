using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public enum GodinaModela
    {
        [Display(Name = "2022")]
        G2022 = 2022,

        [Display(Name = "2023")]
        G2023 = 2023,

        [Display(Name = "2024")]
        G2024 = 2024
    }
}