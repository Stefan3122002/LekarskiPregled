using System.Collections.Generic;
using System.Linq;
using KlasePodataka.Entiteti;

namespace PoslovnaLogika
{
    public class LekarskiPregledPoslovnaLogika
    {
        private PravilaKlijent _servis;

        public LekarskiPregledPoslovnaLogika()
        {
            _servis = new PravilaKlijent();
        }

        public bool ValidirajPregled(LekarskiPregledKlasa p, out string poruka)
        {
            poruka = string.Empty;

            if (string.IsNullOrWhiteSpace(p.ZdravstvenaUstanova) || p.ZdravstvenaUstanova.Trim().Length < _servis.MinUstanovaDuzina())
            {
                poruka = _servis.PorukaMinUstanova();
                return false;
            }

            if (string.IsNullOrWhiteSpace(p.AdresaUstanove) || p.AdresaUstanove.Trim().Length < _servis.MinAdresaDuzina())
            {
                poruka = _servis.PorukaMinAdresa();
                return false;
            }

            if (p.TrajanjePregledaMinuta < _servis.MinTrajanjeMinuta() || p.TrajanjePregledaMinuta > _servis.MaxTrajanjeMinuta())
            {
                poruka = _servis.PorukaTrajanje();
                return false;
            }

            if (p.BrojLekaraUKomisiji < _servis.MinBrojLekara() || p.BrojLekaraUKomisiji > _servis.MaxBrojLekara())
            {
                poruka = _servis.PorukaBrojLekara();
                return false;
            }

            if (p.Zavrsen && _servis.NapomenaObaveznaZaZavrsen() && string.IsNullOrWhiteSpace(p.Napomena))
            {
                poruka = _servis.PorukaNapomenaZavrsen();
                return false;
            }

            return true;
        }

        // ============================================================================
        // POSLOVNO PRAVILO:
        // Ako je obrazovni profil srednje skole za koji kandidat konkurise naveden u
        // listi profila koji zahtijevaju specijalizirane zdravstvene preglede (lista
        // profila se cuva u eksternom XML/JSON parametru - vidi PravilaKlijent i
        // WebServis/XML/Pravila.xml), onda se lekarsko uvjerenje ne moze smatrati
        // validnim bez prilozenih nalaza SVIH dodatnih specijalistickih pregleda
        // propisanih za taj profil.
        // ============================================================================
        public bool JeUvjerenjeValidno(KandidatKlasa kandidat, IEnumerable<PrilozeniNalazKlasa> prilozeniNalazi, out string poruka)
        {
            poruka = string.Empty;

            if (kandidat?.Profil == null)
            {
                poruka = "Obrazovni profil kandidata nije poznat.";
                return false;
            }

            ProfilSaDodatnimPregledimaDto[] profiliSaPravilom = _servis.DajProfileSaDodatnimPregledima();

            ProfilSaDodatnimPregledimaDto pravilo = profiliSaPravilom?
                .FirstOrDefault(pr => string.Equals(pr.Sifra, kandidat.Profil.Sifra, System.StringComparison.OrdinalIgnoreCase));

            // Profil nije na listi profila sa dodatnim pregledima - lekarsko uvjerenje
            // je validno bez dodatnih specijalistickih nalaza.
            if (pravilo == null || pravilo.PotrebniNalazi == null || pravilo.PotrebniNalazi.Length == 0)
                return true;

            var uredniNalazi = new HashSet<string>(
                (prilozeniNalazi ?? Enumerable.Empty<PrilozeniNalazKlasa>())
                    .Where(n => n.Uredan && !string.IsNullOrWhiteSpace(n.VrstaNalaza))
                    .Select(n => n.VrstaNalaza.Trim().ToLower()));

            List<string> nedostajuciNalazi = pravilo.PotrebniNalazi
                .Where(potreban => !uredniNalazi.Contains(potreban.Trim().ToLower()))
                .ToList();

            if (nedostajuciNalazi.Any())
            {
                poruka = "Lekarsko uvjerenje nije validno. Obrazovni profil '" + pravilo.Naziv +
                         "' zahtijeva sledece dodatne specijalisticke preglede sa urednim nalazom: " +
                         string.Join(", ", nedostajuciNalazi) + ".";
                return false;
            }

            return true;
        }
    }
}
