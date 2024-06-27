using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Models;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Prijava()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Prijava(string korisnickoIme, string lozinka)
        {
            if (korisnickoIme == "admin" && lozinka == "admin")
            {
                HttpCookie authCookie = new HttpCookie("AdminKorisnik");
                authCookie.Value = korisnickoIme;
                Response.Cookies.Add(authCookie);

                return RedirectToAction("PopisZaAdmina", "Mobiteli");
            }

            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka");
            return View();
        }

        [HttpPost]
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
