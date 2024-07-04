using System.Linq;
using System.Web.Mvc;
using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;
using System.Data.Entity;
using System.Collections.Generic;
using PagedList;
using System;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Controllers
{
    public class MobiteliController : Controller
    {
        private readonly BazaDbContext bazaPodataka;

        public MobiteliController()
        {
            bazaPodataka = new BazaDbContext();
        }

        public ActionResult Index()
        {
            var mobiteli = bazaPodataka.Mobiteli.ToList();

            return View(mobiteli);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bazaPodataka.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PopisRacuna()
        {
            // Logic to fetch and display list of invoices
            return View();
        }



        public ActionResult PopisZaAdmina(string naziv, string proizvodjac, int? page)
        {
            try
            {
                var mobiteli = bazaPodataka.Mobiteli.ToList();

                if (mobiteli == null || !mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                    return View(new List<Mobitel>().ToPagedList(1, 10));
                }

                // Filtriranje po nazivu
                if (!string.IsNullOrWhiteSpace(naziv))
                {
                    mobiteli = mobiteli.Where(x => x.Naziv != null && x.Naziv.ToUpper().Contains(naziv.ToUpper())).ToList();
                }

                // Filtriranje po proizvodjacu
                if (!string.IsNullOrWhiteSpace(proizvodjac))
                {
                    mobiteli = mobiteli.Where(x => x.Proizvodjac != null && x.Proizvodjac.ToUpper() == proizvodjac.ToUpper()).ToList();
                }

                if (!mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                    return View(new List<Mobitel>().ToPagedList(1, 10));
                }

                int pageSize = 10;
                int pageNumber = (page ?? 1);
                pageNumber = pageNumber < 1 ? 1 : pageNumber;

                return View(mobiteli.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return View(new List<Mobitel>().ToPagedList(1, 10));
            }
        }


        public ActionResult DetaljiZaKorisnika(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("PopisZaKorisnika");
            }

            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            if (mobitel == null)
            {
                return RedirectToAction("PopisZaKorisnika");
            }

            return View(mobitel);
        }

        public ActionResult DetaljiZaAdmina(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("PopisZaAdmina");
            }

            Mobitel mobitel = bazaPodataka.Mobiteli.FirstOrDefault(x => x.ID == id);

            if (mobitel == null)
            {
                return RedirectToAction("PopisZaAdmina");
            }

            return View(mobitel);
        }

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

            // Priprema liste proizvođača iz baze podataka
            var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
            ViewBag.Proizvodjaci = new SelectList(proizvodjaci);

            return View(mobitel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Spremi(Mobitel mobitel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (mobitel.ID != 0)
                    {
                        bazaPodataka.Entry(mobitel).State = EntityState.Modified;
                    }
                    else
                    {
                        bazaPodataka.Mobiteli.Add(mobitel);
                    }
                    bazaPodataka.SaveChanges();

                    return RedirectToAction("PopisZaAdmina");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    // Ponovno pripremite SelectListu za proizvođače u slučaju greške
                    var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
                    ViewBag.Proizvodjaci = new SelectList(proizvodjaci);

                    return View("Azuriraj", mobitel);
                }
            }
            else
            {
                // Ponovno pripremite SelectListu za proizvođače u slučaju nevaljanog modela
                var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
                ViewBag.Proizvodjaci = new SelectList(proizvodjaci);

                return View("Azuriraj", mobitel);
            }
        }

        public ActionResult DodajNoviMobitel()
        {
            // Priprema liste proizvođača iz baze podataka
            var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
            ViewBag.ExistingProizvodjaci = new SelectList(proizvodjaci);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajNoviMobitel(Mobitel mobitel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Dodavanje novog mobitela u bazu podataka
                    bazaPodataka.Mobiteli.Add(mobitel);
                    bazaPodataka.SaveChanges();

                    // Preusmjeri na akciju za prikaz popisa mobitela
                    return RedirectToAction("PopisZaAdmina");
                }
                catch (Exception ex)
                {
                    // Prikaz poruke o grešci u slučaju problema s bazom podataka
                    ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    // Ponovno pripremite SelectListu za proizvođače u slučaju greške
                    var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
                    ViewBag.ExistingProizvodjaci = new SelectList(proizvodjaci);

                    return View(mobitel);
                }
            }
            else
            {
                // Ponovno pripremite SelectListu za proizvođače u slučaju neuspješne validacije
                var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
                ViewBag.ExistingProizvodjaci = new SelectList(proizvodjaci);

                // Ako model nije valjan, vrati se na formu za dodavanje mobitela
                return View(mobitel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Azuriraj(Mobitel mobitel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (mobitel.ID != 0)
                    {
                        bazaPodataka.Entry(mobitel).State = EntityState.Modified;
                    }
                    else
                    {
                        bazaPodataka.Mobiteli.Add(mobitel);
                    }
                    bazaPodataka.SaveChanges();
                    return RedirectToAction("PopisZaAdmina");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    // Priprema SelectListe za proizvođače ako je potrebno
                    var proizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
                    ViewBag.Proizvodjaci = new SelectList(proizvodjaci);

                    if (mobitel.ID == 0)
                    {
                        ViewBag.Title = "Kreiranje mobitela";
                        ViewBag.Novi = true;
                    }
                    else
                    {
                        ViewBag.Title = "Ažuriranje podataka o mobitelu";
                        ViewBag.Novi = false;
                    }

                    return View(mobitel);
                }
            }

            // Priprema SelectListe za proizvođače ako je potrebno
            var existingProizvodjaci = bazaPodataka.Mobiteli.Select(m => m.Proizvodjac).Distinct().ToList();
            ViewBag.Proizvodjaci = new SelectList(existingProizvodjaci);

            if (mobitel.ID == 0)
            {
                ViewBag.Title = "Kreiranje mobitela";
                ViewBag.Novi = true;
            }
            else
            {
                ViewBag.Title = "Ažuriranje podataka o mobitelu";
                ViewBag.Novi = false;
            }

            return View(mobitel);

        }


        // GET: Mobiteli/Brisi/{id}
        public ActionResult Brisi(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("PopisZaAdmina");
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

            // Preusmjeri na akciju za prikaz popisa mobitela ili neku drugu prikladnu stranicu
            return RedirectToAction("PopisZaAdmina");
        }
        public ActionResult PopisZaKorisnika(string naziv, string proizvodjac, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            // Assume that the username is stored in the session
            string KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
            ViewBag.KorisnickoIme = KorisnickoIme;


            try
            {
                var query = bazaPodataka.Mobiteli.AsQueryable();

                if (!string.IsNullOrWhiteSpace(naziv))
                {
                    query = query.Where(x => x.Naziv != null && x.Naziv.ToUpper().Contains(naziv.ToUpper()));
                }

                if (!string.IsNullOrWhiteSpace(proizvodjac))
                {
                    query = query.Where(x => x.Proizvodjac != null && x.Proizvodjac.ToUpper() == proizvodjac.ToUpper());
                }

                var mobiteli = query.ToList();

                if (!mobiteli.Any())
                {
                    ViewBag.Message = "Nema rezultata pretrage!";
                }

                var pagedMobiteli = mobiteli.ToPagedList(pageNumber, pageSize);

                return View(pagedMobiteli);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return View(new List<Mobitel>().ToPagedList(pageNumber, pageSize));
            }
        }
        // GET: /Mobiteli/PrijavaReklamacije
        // GET: /Mobiteli/PrijavaReklamacije
        public ActionResult PrijavaReklamacije(int racunStavkaID)
        {
            var model = new Reklamacija
            {
                RacunStavkaID = racunStavkaID,
                DatumReklamacije = DateTime.Now
            };

            // Postavljanje korisničkog imena iz sesije
            model.KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();

            return View(model);
        }


        // POST: /Mobiteli/PrijavaReklamacije
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrijavaReklamacije(Reklamacija reklamacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string korisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
                    ViewBag.KorisnickoIme = korisnickoIme;
                    reklamacija.DatumReklamacije = DateTime.Now;
                    reklamacija.KorisnickoIme = korisnickoIme; // Postavljanje korisničkog imena

                    var racunStavka = bazaPodataka.RacunStavke.Find(reklamacija.RacunStavkaID);
                    if (racunStavka == null)
                    {
                        ViewBag.ErrorMessage = "RacunStavka nije pronađen.";
                        return View(reklamacija);
                    }

                    reklamacija.NazivMobitela = racunStavka.Mobitel.Naziv;

                    bazaPodataka.Reklamacije.Add(reklamacija);
                    bazaPodataka.SaveChanges();

                    TempData["SuccessMessage"] = "Vaša reklamacija je uspješno prijavljena.";
                    return RedirectToAction("PregledReklamacija");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Došlo je do greške prilikom spremanja reklamacije: " + ex.Message;
                }
            }

            return View(reklamacija);
        }



        // GET: /Mobiteli/PregledReklamacija
        // GET: /Mobiteli/PregledReklamacija
        public ActionResult PregledReklamacija()
        {
            var reklamacije = bazaPodataka.Reklamacije.Include(r => r.RacunStavka.Mobitel).ToList();

            foreach (var reklamacija in reklamacije)
            {
                reklamacija.NazivMobitela = reklamacija.RacunStavka.Mobitel.Naziv;
            }

            return View(reklamacije);
        }



        // GET: /Mobiteli/PregledReklamacija

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BrisiReklamaciju(int id)
        {
            try
            {
                using (var context = new BazaDbContext())
                {
                    var reklamacija = context.Reklamacije.FirstOrDefault(r => r.ReklamacijaID == id);
                    if (reklamacija == null)
                    {
                        TempData["ErrorMessage"] = "Reklamacija nije pronađena.";
                        return RedirectToAction("PregledReklamacija");
                    }

                    context.Reklamacije.Remove(reklamacija);
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Reklamacija je uspješno obrisana.";
                    return RedirectToAction("PregledReklamacija");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Došlo je do pogreške prilikom brisanja reklamacije: " + ex.Message;
                return RedirectToAction("PregledReklamacija");
            }
        }




        public ActionResult PotvrdaBrisanja(int id)
        {
            using (var context = new BazaDbContext())
            {
                var reklamacija = context.Reklamacije.Find(id);
                if (reklamacija == null)
                {
                    return HttpNotFound();
                }
                return View(reklamacija);
            }
        }

        // Dodavanje u košaricu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajUKosaricu(int mobitelID, int kolicina)
        {
            try
            {
                using (var bazaPodataka = new BazaDbContext())
                {
                    var mobitel = bazaPodataka.Mobiteli.Find(mobitelID);
                    if (mobitel == null)
                    {
                        return Json(new { success = false, message = "Mobitel nije pronađen." });
                    }

                    // Ako je količina manja od 1, postavi na 1
                    if (kolicina < 1)
                    {
                        kolicina = 1;
                    }

                    // Dohvati kosaricu iz sesije
                    var kosaricaMobiteli = Session["Kosarica"] as List<Mobitel>;
                    if (kosaricaMobiteli == null)
                    {
                        kosaricaMobiteli = new List<Mobitel>();
                    }

                    // Provjeri postoji li mobitel već u košarici
                    var postojeciMobitel = kosaricaMobiteli.FirstOrDefault(m => m.ID == mobitelID);
                    if (postojeciMobitel != null)
                    {
                        // Ako postoji, samo ažuriraj količinu
                        postojeciMobitel.Kolicina += kolicina;
                    }
                    else
                    {
                        // Inače dodaj novi mobitel u košaricu
                        mobitel.Kolicina = kolicina;
                        kosaricaMobiteli.Add(mobitel);
                    }

                    // Spremi promjene u sesiju
                    Session["Kosarica"] = kosaricaMobiteli;

                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Došlo je do greške: " + ex.Message });
            }
        }


        public ActionResult Kosarica()
        {
            string KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
            ViewBag.KorisnickoIme = KorisnickoIme;
           
            var kosaricaMobiteli = Session["Kosarica"] as List<Mobitel>;
            if (kosaricaMobiteli == null)
            {
                kosaricaMobiteli = new List<Mobitel>();
                Session["Kosarica"] = kosaricaMobiteli;
            }

            return View(kosaricaMobiteli);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ObrisiIzKosarice(int mobitelID)
        {
            var kosaricaMobiteli = Session["Kosarica"] as List<Mobitel>;
            if (kosaricaMobiteli != null)
            {
                var mobitel = kosaricaMobiteli.FirstOrDefault(m => m.ID == mobitelID);

                if (mobitel != null)
                {
                    kosaricaMobiteli.Remove(mobitel);
                    Session["Kosarica"] = kosaricaMobiteli;

                    // Ako je sesija prazna nakon brisanja, postavi je na null
                    if (kosaricaMobiteli.Count == 0)
                    {
                        Session["Kosarica"] = null;
                    }
                }
            }

            return RedirectToAction("Kosarica"); // Preusmjeri na akciju Kosarica za prikaz ažurirane košarice
        }

        public ActionResult PrikaziRacun(int? id)
        {

            try
            {
                using (var bazaPodataka = new BazaDbContext())
                {
                    Racun racun = null;
                    string KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
                    ViewBag.KorisnickoIme = KorisnickoIme;

                    if (!id.HasValue)
                    {
                        var mobiteliUKosarici = Session["Kosarica"] as List<Mobitel>;
                        if (mobiteliUKosarici == null || mobiteliUKosarici.Count == 0)
                        {
                            return RedirectToAction("Kosarica");
                        }

                        decimal ukupanIznos = mobiteliUKosarici.Sum(m => (decimal)m.Cijena * m.Kolicina);

                        racun = new Racun
                        {
                            DatumKupnje = DateTime.Now,
                            KorisnickoIme = KorisnickoIme,
                            UkupanIznos = ukupanIznos,
                            RacunStavke = mobiteliUKosarici.Select(m => new RacunStavka
                            {
                                MobitelID = m.ID,
                                Kolicina = m.Kolicina,
                                Cijena = (decimal)m.Cijena,
                                UkupnaCijena = (decimal)m.Cijena * m.Kolicina
                            }).ToList()
                        };

                        // Logiranje podataka prije spremanja
                        System.Diagnostics.Debug.WriteLine("Racun Data:");
                        System.Diagnostics.Debug.WriteLine($"DatumKupnje: {racun.DatumKupnje}");
                        System.Diagnostics.Debug.WriteLine($"KorisnickoIme: {racun.KorisnickoIme}");
                        System.Diagnostics.Debug.WriteLine($"UkupanIznos: {racun.UkupanIznos}");
                        foreach (var stavka in racun.RacunStavke)
                        {
                            System.Diagnostics.Debug.WriteLine($"Stavka - MobitelID: {stavka.MobitelID}, Kolicina: {stavka.Kolicina}, Cijena: {stavka.Cijena}, UkupnaCijena: {stavka.UkupnaCijena}");
                        }

                        bazaPodataka.Racuni.Add(racun);
                        bazaPodataka.SaveChanges();

                        Session["Kosarica"] = null;
                        return RedirectToAction("PrikaziRacun", new { id = racun.RacunID });
                    }
                    else
                    {
                        racun = bazaPodataka.Racuni
                                            .Include(r => r.RacunStavke.Select(rs => rs.Mobitel))
                                            .FirstOrDefault(r => r.RacunID == id.Value);

                        if (racun == null)
                        {
                            return HttpNotFound();
                        }

                        ViewBag.KorisnickoIme = racun.KorisnickoIme;
                    }

                    return View(racun);
                }
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.ToString() : "No inner exception";

                // Detaljno logiranje
                System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {innerException}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                return Json(new { success = false, message = "Došlo je do greške prilikom generiranja računa: " + ex.Message, innerError = innerException }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult PopisSvihRacuna()
        {
           
            try
            {
                using (var bazaPodataka = new BazaDbContext())
                {
                    // Fetch all invoices including related details (e.g., user's username)
                    var racuni = bazaPodataka.Racuni.ToList();

                    return View(racuni);
                }
            }
            catch (Exception ex)
            {
                // Log and handle exceptions appropriately
                ViewBag.ErrorMessage = "Došlo je do greške prilikom dohvaćanja popisa računa.";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.ToString()}");
                return View(new List<Racun>());
            }
        }
        public ActionResult DetaljiRacuna(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                using (var bazaPodataka = new BazaDbContext())
                {
                    var racun = bazaPodataka.Racuni
                                            .Include(r => r.RacunStavke.Select(rs => rs.Mobitel))
                                            .FirstOrDefault(r => r.RacunID == id);

                    if (racun == null)
                    {
                        return HttpNotFound();
                    }

                    return View(racun);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Došlo je do greške prilikom dohvaćanja detalja računa ID {id}.";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.ToString()}");
                return View(new Racun());
            }
        }

        public ActionResult IzbrisiRacun(int id)
        {
            try
            {
                using (var bazaPodataka = new BazaDbContext())
                {
                    var racun = bazaPodataka.Racuni
                                            .Include(r => r.RacunStavke)
                                            .FirstOrDefault(r => r.RacunID == id);

                    if (racun == null)
                    {
                        TempData["ErrorMessage"] = $"Račun ID {id} nije pronađen.";
                        return RedirectToAction("PopisSvihRacuna");
                    }

                    // Brisanje povezanih stavki računa
                    bazaPodataka.RacunStavke.RemoveRange(racun.RacunStavke);

                    // Brisanje računa
                    bazaPodataka.Racuni.Remove(racun);
                    bazaPodataka.SaveChanges();

                    TempData["SuccessMessage"] = "Račun je uspješno izbrisan.";
                    return RedirectToAction("PopisSvihRacuna");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Došlo je do greške prilikom brisanja računa ID {id}.";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.ToString()}");
                return RedirectToAction("PopisSvihRacuna");
            }
        }
    



    public ActionResult MojeKupnje()
        {
            try
            {
                string KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
                ViewBag.KorisnickoIme = KorisnickoIme;

                List<Racun> racuni = new List<Racun>();

                using (var bazaPodataka = new BazaDbContext())
                {
                    // Učitaj račune s povezanim stavkama
                    racuni = bazaPodataka.Racuni
                                         .Include(r => r.RacunStavke) // Učitaj povezane stavke računa
                                         .Where(r => r.KorisnickoIme == KorisnickoIme)
                                         .ToList();
                }

                System.Diagnostics.Debug.WriteLine($"Found {racuni.Count} invoices for user {KorisnickoIme}");

                return View(racuni);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Došlo je do greške prilikom dohvaćanja vaših kupnji.";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.ToString()}");
                return View(new List<Racun>());
            }
        }




        public ActionResult DetaljiRacunaZaKupca(int? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                string KorisnickoIme = HttpContext.Session["KorisnickoIme"]?.ToString();
                ViewBag.KorisnickoIme = KorisnickoIme;

                using (var bazaPodataka = new BazaDbContext())
                {
                    var racun = bazaPodataka.Racuni
                                            .Include(r => r.RacunStavke.Select(rs => rs.Mobitel))
                                            .FirstOrDefault(r => r.RacunID == id && r.KorisnickoIme == KorisnickoIme);

                    if (racun == null)
                    {
                        return HttpNotFound();
                    }

                    return View(racun);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Došlo je do greške prilikom dohvaćanja detalja računa ID {id}.";
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.ToString()}");
                return View(new Racun());
            }
        }

    }
}