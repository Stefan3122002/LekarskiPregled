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
                poruka = "Lekarsko uverenje nije validno. Obrazovni profil '" + pravilo.Naziv +
                         "' zahteva sledece dodatne specijalisticke preglede sa urednim nalazom: " +
                         string.Join(", ", nedostajuciNalazi) + ".";
                return false;
            }

            return true;
        }
    }
}
