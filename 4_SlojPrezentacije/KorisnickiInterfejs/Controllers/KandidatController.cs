using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KlasePodataka.Entiteti;
using KlasePodataka.Repozitorijumi;
using PoslovnaLogika;
using KorisnickiInterfejs.Models;

namespace KorisnickiInterfejs.Controllers
{
    public class KandidatController : Controller
    {
        private bool JePrijavljen() => Session["KorisnikID"] != null;

        public ActionResult Index(string filter = "")
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KandidatRepozitorijum repo = new KandidatRepozitorijum(kontekst);
                var lista = repo.DajSve();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    string f = filter.Trim().ToLower();
                    lista = lista.Where(k =>
                        k.Ime.ToLower().Contains(f) ||
                        k.Prezime.ToLower().Contains(f) ||
                        k.JMBG.Contains(f)).ToList();
                }

                ViewBag.Filter = filter;
                return View(lista.ToList());
            }
        }

        public ActionResult Unos()
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            KandidatViewModel model = new KandidatViewModel
            {
                DatumRodjenja = System.DateTime.Today.AddYears(-15),
                Aktivan       = true,
                VrstaUpisa    = "Redovan upis"
            };
            PopuniDropDowne(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unos(KandidatViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDowne(model);
                return View(model);
            }

            KandidatPoslovnaLogika logika = new KandidatPoslovnaLogika();
            KandidatKlasa k = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajKandidata(k, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDowne(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KandidatRepozitorijum repo = new KandidatRepozitorijum(kontekst);

                if (repo.PostojiJMBG(k.JMBG))
                {
                    ModelState.AddModelError("JMBG", "Kandidat sa ovim JMBG vec postoji.");
                    PopuniDropDowne(model);
                    return View(model);
                }

                repo.Dodaj(k);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Kandidat je uspjesno dodat.";
            return RedirectToAction("Index");
        }

        public ActionResult Izmena(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KandidatRepozitorijum repo = new KandidatRepozitorijum(kontekst);
                KandidatKlasa k = repo.DajPoId(id);
                if (k == null) return HttpNotFound();

                KandidatViewModel model = MapirajUViewModel(k);
                PopuniDropDowne(model);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmena(KandidatViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDowne(model);
                return View(model);
            }

            KandidatPoslovnaLogika logika = new KandidatPoslovnaLogika();
            KandidatKlasa k = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajKandidata(k, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDowne(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KandidatRepozitorijum repo = new KandidatRepozitorijum(kontekst);

                if (repo.PostojiJMBG(k.JMBG, k.KandidatID))
                {
                    ModelState.AddModelError("JMBG", "Kandidat sa ovim JMBG vec postoji.");
                    PopuniDropDowne(model);
                    return View(model);
                }

                repo.Izmeni(k);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Kandidat je uspjesno izmijenjen.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisanje(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                KandidatRepozitorijum repo = new KandidatRepozitorijum(kontekst);
                repo.Obrisi(id);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Kandidat je uspjesno obrisan.";
            return RedirectToAction("Index");
        }

        private void PopuniDropDowne(KandidatViewModel model)
        {
            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                model.SkoleSeznam = kontekst.Skole.ToList()
                    .Select(s => new SelectListItem { Value = s.SkolaID.ToString(), Text = s.Naziv });
                model.ProfiliSeznam = kontekst.Profili.ToList()
                    .Select(p => new SelectListItem { Value = p.ProfilID.ToString(), Text = p.Naziv + " (" + p.Sifra + ")" });
            }
        }

        private KandidatKlasa MapirajUEntitet(KandidatViewModel m) =>
            new KandidatKlasa
            {
                KandidatID    = m.KandidatID,
                Ime           = m.Ime?.Trim(),
                Prezime       = m.Prezime?.Trim(),
                JMBG          = m.JMBG?.Trim(),
                DatumRodjenja = m.DatumRodjenja,
                SkolaID       = m.SkolaID,
                ProfilID      = m.ProfilID,
                VrstaUpisa    = string.IsNullOrWhiteSpace(m.VrstaUpisa) ? "Redovan upis" : m.VrstaUpisa.Trim(),
                Aktivan       = m.Aktivan
            };

        private KandidatViewModel MapirajUViewModel(KandidatKlasa k) =>
            new KandidatViewModel
            {
                KandidatID    = k.KandidatID,
                Ime           = k.Ime,
                Prezime       = k.Prezime,
                JMBG          = k.JMBG,
                DatumRodjenja = k.DatumRodjenja,
                SkolaID       = k.SkolaID,
                ProfilID      = k.ProfilID,
                VrstaUpisa    = k.VrstaUpisa,
                Aktivan       = k.Aktivan,
                SkolaNaziv    = k.Skola?.Naziv,
                ProfilNaziv   = k.Profil?.Naziv,
                ProfilSifra   = k.Profil?.Sifra
            };
    }
}
