using System.Web.Mvc;

namespace Projekt_PAUP_Prodaja_Mobitela_Petar_Čanadi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

      
            public ActionResult Onama()
            {
                ViewBag.Title = "O nama";
                ViewBag.Message = "Dobrodošli u našu trgovinu mobitelima!";
                return View("Onama");
            }
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
