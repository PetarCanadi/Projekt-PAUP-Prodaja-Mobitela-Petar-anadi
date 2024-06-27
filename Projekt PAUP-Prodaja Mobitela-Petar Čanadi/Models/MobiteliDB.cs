using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models
{
    public class MobiteliDB
    {
        private static List<Mobitel> lista = new List<Mobitel>();
        private bool listaInicijalizirana = false;

        public MobiteliDB()
        {
            if (listaInicijalizirana == false)
            {
                listaInicijalizirana = true;
                lista.Add(new Mobitel());
                lista.Add(new Mobitel());
                lista.Add(new Mobitel());
                lista.Add(new Mobitel());
                lista.Add(new Mobitel());

            }
            lista.Add(new Mobitel()
            {
                ID = 1,
                Naziv = "Galaxy S21",
                Proizvodjac = "Samsung",
                Cijena = 1000,
                Opis = "Android mobitel",
              


            }
            );

            lista.Add(new Mobitel()
            {
                ID = 2,
                Naziv = "A54",
                Proizvodjac = "Samsung",
                Cijena = 500,
                Opis = "Android mobitel",
               
            }
            );

            lista.Add(new Mobitel()
            {
                ID = 3,
                Naziv = "Iphone 15",
                Proizvodjac = "Apple",
                Cijena = 1100,
                Opis = "iOS mobitel",
                
            }
            );

            lista.Add(new Mobitel()
            {
                ID = 4,
                Naziv = "Iphone 15 Pro",
                Proizvodjac = "Apple",
                Cijena = 1300,
                Opis = "iOS mobitel",
               
            }
            );
        }

        public List<Mobitel> VratiListu()
        {
            return lista;
        }
        public void AzurirajMobitela(Mobitel mobitel)
        {
            int mobitelIndex = lista.FindIndex(x => x.ID == mobitel.ID);
            lista[mobitelIndex] = mobitel;
        }
    }
}