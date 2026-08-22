using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KlasePodataka.Entiteti;
using KlasePodataka.Repozitorijumi;
using PoslovnaLogika;
using KorisnickiInterfejs.Models;

namespace KorisnickiInterfejs.Controllers
{
    public class PrilozeniNalazController : Controller
    {
        private bool JePrijavljen() => Session["KorisnikID"] != null;

        public ActionResult Index()
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                PrilozeniNalazRepozitorijum repo = new PrilozeniNalazRepozitorijum(kontekst);
                return View(repo.DajSve().ToList());
            }
        }

        public ActionResult Unos(int? pregledId = null)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            PrilozeniNalazViewModel model = new PrilozeniNalazViewModel
            {
                PregledID   = pregledId ?? 0,
                DatumNalaza = System.DateTime.Today,
                Uredan      = true
            };
            PopuniDropDowne(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unos(PrilozeniNalazViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDowne(model);
                return View(model);
            }

            PrilozeniNalazPoslovnaLogika logika = new PrilozeniNalazPoslovnaLogika();
            PrilozeniNalazKlasa n = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajNalaz(n, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDowne(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                PrilozeniNalazRepozitorijum repo = new PrilozeniNalazRepozitorijum(kontekst);
                repo.Dodaj(n);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Nalaz je uspjesno unesen.";
            return RedirectToAction("Index");
        }

        public ActionResult Izmena(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                PrilozeniNalazRepozitorijum repo = new PrilozeniNalazRepozitorijum(kontekst);
                PrilozeniNalazKlasa n = repo.DajPoId(id);
                if (n == null) return HttpNotFound();

                PrilozeniNalazViewModel model = MapirajUViewModel(n);
                PopuniDropDowne(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmena(PrilozeniNalazViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDowne(model);
                return View(model);
            }

            PrilozeniNalazPoslovnaLogika logika = new PrilozeniNalazPoslovnaLogika();
            PrilozeniNalazKlasa n = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajNalaz(n, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDowne(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                PrilozeniNalazRepozitorijum repo = new PrilozeniNalazRepozitorijum(kontekst);
                repo.Izmeni(n);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Nalaz je uspjesno izmijenjen.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisanje(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                PrilozeniNalazRepozitorijum repo = new PrilozeniNalazRepozitorijum(kontekst);
                repo.Obrisi(id);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Nalaz je uspjesno obrisan.";
            return RedirectToAction("Index");
        }

        private void PopuniDropDowne(PrilozeniNalazViewModel model)
        {
            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                model.PregledSeznam = kontekst.Pregledi
                    .ToList()
                    .Select(p => new SelectListItem
                    {
                        Value = p.PregledID.ToString(),
                        Text  = "#" + p.PregledID + " - " + p.DatumPregleda.ToString("dd.MM.yyyy")
                    });
            }
        }

        private PrilozeniNalazKlasa MapirajUEntitet(PrilozeniNalazViewModel m) =>
            new PrilozeniNalazKlasa
            {
                NalazID     = m.NalazID,
                PregledID   = m.PregledID,
                VrstaNalaza = m.VrstaNalaza?.Trim(),
                DatumNalaza = m.DatumNalaza,
                Ustanova    = m.Ustanova?.Trim(),
                Uredan      = m.Uredan,
                Napomena    = string.IsNullOrWhiteSpace(m.Napomena) ? null : m.Napomena.Trim()
            };

        private PrilozeniNalazViewModel MapirajUViewModel(PrilozeniNalazKlasa n) =>
            new PrilozeniNalazViewModel
            {
                NalazID                   = n.NalazID,
                PregledID                 = n.PregledID,
                VrstaNalaza               = n.VrstaNalaza,
                DatumNalaza               = n.DatumNalaza,
                Ustanova                  = n.Ustanova,
                Uredan                    = n.Uredan,
                Napomena                  = n.Napomena,
                PregledKandidatImePrezime = n.Pregled?.Kandidat?.ImePrezime,
                PregledDatum              = n.Pregled?.DatumPregleda ?? default(System.DateTime)
            };
    }
}
