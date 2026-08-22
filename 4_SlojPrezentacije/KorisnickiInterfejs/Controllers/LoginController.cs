using System.Web.Mvc;
using KlasePodataka;
using KlasePodataka.Repozitorijumi;
using KorisnickiInterfejs.Models;

namespace KorisnickiInterfejs.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (Session["KorisnikID"] != null)
                return RedirectToAction("Index", "LekarskiPregled");
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KorisnikRepozitorijum repo = new KorisnikRepozitorijum(kontekst);
                var korisnik = repo.DajPoKredencijalima(model.Email, model.Lozinka);

                if (korisnik == null)
                {
                    ModelState.AddModelError("", "Pogresan email ili lozinka.");
                    return View(model);
                }

                Session["KorisnikID"] = korisnik.KorisnikID;
                Session["ImePrezime"] = korisnik.ImePrezime;
                Session["Uloga"]      = korisnik.Uloga;
            }

            return RedirectToAction("Index", "LekarskiPregled");
        }

        public ActionResult Odjava()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
