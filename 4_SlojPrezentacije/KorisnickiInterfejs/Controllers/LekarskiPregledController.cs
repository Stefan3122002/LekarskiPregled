using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KlasePodataka.Entiteti;
using KlasePodataka.Repozitorijumi;
using PoslovnaLogika;
using KorisnickiInterfejs.Models;

namespace KorisnickiInterfejs.Controllers
{
    public class LekarskiPregledController : Controller
    {
        private bool JePrijavljen() => Session["KorisnikID"] != null;

        // ===== INDEX =====

        public ActionResult Index(string filter = "")
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                ILekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                IEnumerable<LekarskiPregledKlasa> lista = repo.DajSaFilterom(filter);
                ViewBag.Filter = filter;
                return View(lista.ToList());
            }
        }

        // ===== UNOS - GET =====

        public ActionResult Unos()
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            LekarskiPregledViewModel model = new LekarskiPregledViewModel
            {
                DatumPregleda          = DateTime.Now,
                TrajanjePregledaMinuta = 30,
                BrojLekaraUKomisiji    = 2
            };
            PopuniDropDown(model);
            return View(model);
        }

        // ===== UNOS - POST =====

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unos(LekarskiPregledViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDown(model);
                return View(model);
            }

            LekarskiPregledPoslovnaLogika logika = new LekarskiPregledPoslovnaLogika();
            LekarskiPregledKlasa p = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajPregled(p, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDown(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                ILekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                repo.Dodaj(p);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Lekarski pregled je uspjesno unesen.";
            return RedirectToAction("Index");
        }

        // ===== IZMENA - GET =====

        public ActionResult Izmena(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                ILekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                LekarskiPregledKlasa p = repo.DajPoId(id);
                if (p == null) return HttpNotFound();

                LekarskiPregledViewModel model = MapirajUViewModel(p);
                PopuniDropDown(model);
                return View(model);
            }
        }

        // ===== IZMENA - POST =====

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmena(LekarskiPregledViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                PopuniDropDown(model);
                return View(model);
            }

            LekarskiPregledPoslovnaLogika logika = new LekarskiPregledPoslovnaLogika();
            LekarskiPregledKlasa p = MapirajUEntitet(model);
            string poruka;

            if (!logika.ValidirajPregled(p, out poruka))
            {
                ModelState.AddModelError("", poruka);
                PopuniDropDown(model);
                return View(model);
            }

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                ILekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                repo.Izmeni(p);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Lekarski pregled je uspjesno izmijenjen.";
            return RedirectToAction("Index");
        }

        // ===== BRISANJE =====

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisanje(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                LekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                repo.ObrisiSaKaskadnim(id);
                repo.Sacuvaj();
            }

            TempData["Poruka"] = "Lekarski pregled je uspjesno obrisan.";
            return RedirectToAction("Index");
        }

        // ===== STAMPA (detalji jednog pregleda + provjera poslovnog pravila) =====

        public ActionResult Stampa(int id)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                LekarskiPregledRepozitorijum repo = new LekarskiPregledRepozitorijum(kontekst);
                LekarskiPregledKlasa p = repo.DajSaNalazima(id);
                if (p == null) return HttpNotFound();

                LekarskiPregledPoslovnaLogika logika = new LekarskiPregledPoslovnaLogika();
                string poruka;
                bool validno = logika.JeUvjerenjeValidno(p.Kandidat, p.PrilozeniNalazi, out poruka);

                ViewBag.UvjerenjeValidno = validno;
                ViewBag.PorukaValidnosti = poruka;

                return View(p);
            }
        }

        // ===== STAMPA PARAMETARSKA =====

        public ActionResult StampaParametarska()
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            StampaLekarskiPregledViewModel model = new StampaLekarskiPregledViewModel();
            PopuniProfileSeznam(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StampaParametarska(StampaLekarskiPregledViewModel model)
        {
            if (!JePrijavljen()) return RedirectToAction("Index", "Login");

            string connStr = System.Configuration.ConfigurationManager
                .ConnectionStrings["LekarskiPreglediConn"].ConnectionString;

            KlasePodataka.SPKlase.SPLekarskiPregledDBKlasa sp =
                new KlasePodataka.SPKlase.SPLekarskiPregledDBKlasa(connStr);

            model.Rezultati = sp.DajPreglededZaStampu(model.DatumOd, model.DatumDo, model.ProfilID);
            PopuniProfileSeznam(model);
            return View(model);
        }

        // ===== POMOCNE METODE =====

        private void PopuniDropDown(LekarskiPregledViewModel model)
        {
            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                model.KandidatiSeznam = kontekst.Kandidati.ToList()
                    .Select(k => new SelectListItem
                    {
                        Value = k.KandidatID.ToString(),
                        Text  = k.Ime + " " + k.Prezime + " (" + k.JMBG + ")"
                    });
            }
        }

        private void PopuniProfileSeznam(StampaLekarskiPregledViewModel model)
        {
            using (LekarskiPreglediKontekst kontekst = new LekarskiPreglediKontekst())
            {
                var stavke = kontekst.Profili.ToList()
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProfilID.ToString(),
                        Text  = p.Naziv
                    }).ToList();
                stavke.Insert(0, new SelectListItem { Value = "", Text = "-- Svi profili --" });
                model.ProfiliSeznam = stavke;
            }
        }

        private LekarskiPregledKlasa MapirajUEntitet(LekarskiPregledViewModel m) =>
            new LekarskiPregledKlasa
            {
                PregledID              = m.PregledID,
                KandidatID             = m.KandidatID,
                DatumPregleda          = m.DatumPregleda,
                ZdravstvenaUstanova    = m.ZdravstvenaUstanova?.Trim(),
                AdresaUstanove         = m.AdresaUstanove?.Trim(),
                TrajanjePregledaMinuta = m.TrajanjePregledaMinuta,
                BrojLekaraUKomisiji    = m.BrojLekaraUKomisiji,
                Zavrsen                = m.Zavrsen,
                Napomena               = string.IsNullOrWhiteSpace(m.Napomena) ? null : m.Napomena.Trim()
            };

        private LekarskiPregledViewModel MapirajUViewModel(LekarskiPregledKlasa p) =>
            new LekarskiPregledViewModel
            {
                PregledID              = p.PregledID,
                KandidatID             = p.KandidatID,
                DatumPregleda          = p.DatumPregleda,
                ZdravstvenaUstanova    = p.ZdravstvenaUstanova,
                AdresaUstanove         = p.AdresaUstanove,
                TrajanjePregledaMinuta = p.TrajanjePregledaMinuta,
                BrojLekaraUKomisiji    = p.BrojLekaraUKomisiji,
                Zavrsen                = p.Zavrsen,
                Napomena               = p.Napomena,
                KandidatImePrezime     = p.Kandidat?.ImePrezime,
                SkolaNaziv             = p.Kandidat?.Skola?.Naziv,
                ProfilNaziv            = p.Kandidat?.Profil?.Naziv
            };
    }
}
