using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Controllers
{
    public class KorisniciController : Controller
    {
        private readonly BazaDbContext _context;

        public KorisniciController()
        {
            _context = new BazaDbContext();
        }

        [HttpGet]
        public ActionResult Prijava()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Prijava(PrijavaKorisnika prijava)
        {
            if (ModelState.IsValid)
            {
                var korisnik = _context.PrijavaKorisnika
     .FirstOrDefault(k => k.KorisnickoIme == prijava.KorisnickoIme && k.Lozinka == prijava.Lozinka);


                if (korisnik != null)
                {
                    FormsAuthentication.SetAuthCookie(korisnik.KorisnickoIme, false);
                    return RedirectToAction("PopisZaKorisnika", "Mobiteli");
                }
                else
                {
                    ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka");
                }
            }
            return View(prijava);
        }



        [HttpGet]
        public ActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registracija(RegistracijaKorisnika model)
        {
            if (ModelState.IsValid)
            {
                var korisnik = new PrijavaKorisnika
                {
                    KorisnickoIme = model.KorisnickoIme,
                    Lozinka = model.Lozinka
                };

                _context.PrijavaKorisnika.Add(korisnik);
                _context.SaveChanges();

                return RedirectToAction("Prijava");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
