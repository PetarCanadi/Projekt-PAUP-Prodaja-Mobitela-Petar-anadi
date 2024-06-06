
using iTextSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;





namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Controllers
{
    public class MobiteliController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();
        private BazaDbContext _context;


        // GET: Mobiteli
        public ActionResult Index()
        {
            ViewBag.Title = "Popis Dostupnih mobitela za kupnju";
            ViewBag.Mobitel = "Mobitel";
            return View();
        }

        // GET: Mobiteli/Popis
        public ActionResult Popis(string naziv, string proizvodjac)
        {
            try
            {
                var mobiteli = bazaPodataka.Mobiteli.ToList();

                if (mobiteli == null || !mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                    return View(new List<Mobitel>());
                }

                if (!string.IsNullOrWhiteSpace(naziv))
                {
                    mobiteli = mobiteli.Where(x => x.Naziv.ToUpper().Contains(naziv.ToUpper())).ToList();
                }

                if (!string.IsNullOrWhiteSpace(proizvodjac))
                {
                    mobiteli = mobiteli.Where(x => x.Proizvodjac.ToUpper() == proizvodjac.ToUpper()).ToList();
                }

                if (!mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                }

                return View(mobiteli);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return View(new List<Mobitel>());
            }
        }



        // GET: Mobiteli/Detalji/{id}
        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            if (mobitel == null)
            {
                return RedirectToAction("Popis");
            }

            return View(mobitel);
        }

        // GET: Mobiteli/Azuriraj/{id}
        public ActionResult Azuriraj(int? id)
        {
            Mobitel mobitel;
            if (!id.HasValue)
            {
                mobitel = new Mobitel();
                ViewBag.Title = "Kreiranje mobitela";
                ViewBag.Novi = true;
            }
            else
            {
                mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

                if (mobitel == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Title = "Ažuriranje podataka o mobitelima";
                ViewBag.Novi = false;
            }
            return View(mobitel);
        }

        // POST: Mobiteli/Azuriraj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Mobitel m)
        {
            if (ModelState.IsValid)
            {
                if (m.ID != 0)
                {
                    bazaPodataka.Entry(m).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    bazaPodataka.Mobiteli.Add(m);
                }
                bazaPodataka.SaveChanges();
                return RedirectToAction("Popis");
            }

            if (m.ID == 0)
            {
                ViewBag.Title = "Kreiranje mobitela";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o mobitelu";
                ViewBag.Novi = false;
            }

            return View(m);
        }

        // GET: Mobiteli/Brisi/{id}
        public ActionResult Brisi(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            Mobitel m = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);
            if (m == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja mobitela";
            return View(m);
        }

        // POST: Mobiteli/Brisi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int id)
        {
            Mobitel m = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);
            if (m == null)
            {
                return HttpNotFound();
            }

            bazaPodataka.Mobiteli.Remove(m);
            bazaPodataka.SaveChanges();
            return View("BrisiStatus");
        }
        public ActionResult PopisZaKorisnika(string naziv, string proizvodjac)
        {
            try
            {
                // Proverite da li je bazaPodataka null
                if (bazaPodataka == null)
                {
                    ViewBag.Message = "Baza podataka nije dostupna.";
                    return View(new List<Mobitel>());
                }

                // Proverite da li je lista Mobiteli null ili prazna
                var mobiteli = bazaPodataka.Mobiteli.ToList();
                if (mobiteli == null || !mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                    return View(new List<Mobitel>());
                }

                // Filtriranje po nazivu
                if (!string.IsNullOrWhiteSpace(naziv))
                {
                    mobiteli = mobiteli.Where(x => x.Naziv.ToUpper().Contains(naziv.ToUpper())).ToList();
                }

                // Filtriranje po proizvodjacu
                if (!string.IsNullOrWhiteSpace(proizvodjac))
                {
                    mobiteli = mobiteli.Where(x => x.Proizvodjac.ToUpper() == proizvodjac.ToUpper()).ToList();
                }

                // Ako nema rezultata nakon filtriranja, postavite odgovarajuću poruku
                if (!mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                }

                return View(mobiteli);
            }
            catch (Exception ex)
            {
                // Prikaz unutrašnjeg izuzetka
                ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return View(new List<Mobitel>());
            }
        }
        public ActionResult PotvrdaKupovine(int id)
        {
            var mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);
            if (mobitel == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PotvrdaKupovine", mobitel);
        }

        public ActionResult Popis()
        {
            var mobiteli = bazaPodataka.Mobiteli.ToList();
            return View("Kupnja", mobiteli);
        }
        public ActionResult Kupi(int id)
        {
            // Pronađi mobitel iz baze podataka prema ID-u
            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            // Ako mobitel nije pronađen, preusmjeri na popis mobitela
            if (mobitel == null)
            {
                return HttpNotFound();
            }

            // Preusmjeri na stranicu za detalje kupnje
            return RedirectToAction("detaljikupnje", new { id = mobitel.ID });
        }

        public ActionResult detaljikupnje(int id)
        {
            // Pronađi mobitel iz baze podataka prema ID-u
            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            // Ako mobitel nije pronađen, vrati HttpNotFound
            if (mobitel == null)
            {
                return HttpNotFound();
            }

            // Vrati View s detaljima o mobitelu
            return View(mobitel);
        }


        public ActionResult GenerirajRacun(int id)
        {
            // Pronađi mobitel iz baze podataka prema ID-u
            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            // Ako mobitel nije pronađen, vrati HTTP 404
            if (mobitel == null)
            {
                return HttpNotFound();
            }

            // Vrati View s detaljima mobitela i generiranim računom
            return View(mobitel);
        }
        [HttpPost]
        public ActionResult PrijaviReklamaciju(Reklamacija model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Instanciranje konteksta baze podataka
                    BazaDbContext context = new BazaDbContext();

                    // Dodavanje reklamacije u kontekst baze podataka
                    context.Reklamacije.Add(model);

                    // Spremanje promjena u bazi podataka
                    context.SaveChanges();

                    ViewBag.Message = "Reklamacija uspješno prijavljena.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Došlo je do pogreške prilikom spremanja reklamacije: " + ex.Message;
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
