using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.DBKlase
{
    public class LekarskiPregledDBKlasa
    {
        private LekarskiPreglediKontekst _ctx;

        public LekarskiPregledDBKlasa(LekarskiPreglediKontekst kontekst)
        {
            _ctx = kontekst;
        }

        public IEnumerable<LekarskiPregledKlasa> DajSve() =>
            _ctx.Pregledi.Include(p => p.Kandidat.Skola)
                 .Include(p => p.Kandidat.Profil)
                 .OrderByDescending(p => p.DatumPregleda).ToList();

        public LekarskiPregledKlasa DajPoId(int id) =>
            _ctx.Pregledi.Include(p => p.Kandidat.Skola)
                 .Include(p => p.Kandidat.Profil)
                 .FirstOrDefault(p => p.PregledID == id);

        public IEnumerable<LekarskiPregledKlasa> DajSaFilterom(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return DajSve();

            string f = filter.Trim().ToLower();
            return _ctx.Pregledi
                .Include(p => p.Kandidat.Skola)
                .Include(p => p.Kandidat.Profil)
                .Where(p => p.Kandidat.Ime.ToLower().Contains(f) ||
                            p.Kandidat.Prezime.ToLower().Contains(f) ||
                            p.ZdravstvenaUstanova.ToLower().Contains(f))
                .OrderByDescending(p => p.DatumPregleda)
                .ToList();
        }

        public LekarskiPregledKlasa DajSaNalazima(int id) =>
            _ctx.Pregledi
                .Include(p => p.Kandidat.Skola)
                .Include(p => p.Kandidat.Profil)
                .Include(p => p.PrilozeniNalazi)
                .FirstOrDefault(p => p.PregledID == id);

        public void Dodaj(LekarskiPregledKlasa p) => _ctx.Pregledi.Add(p);

        public void Izmeni(LekarskiPregledKlasa p)
        {
            var e = _ctx.Pregledi.Find(p.PregledID);
            if (e == null) return;
            e.KandidatID             = p.KandidatID;
            e.DatumPregleda          = p.DatumPregleda;
            e.ZdravstvenaUstanova    = p.ZdravstvenaUstanova;
            e.AdresaUstanove         = p.AdresaUstanove;
            e.TrajanjePregledaMinuta = p.TrajanjePregledaMinuta;
            e.BrojLekaraUKomisiji    = p.BrojLekaraUKomisiji;
            e.Zavrsen                = p.Zavrsen;
            e.Napomena               = p.Napomena;
        }

        public void ObrisiSaKaskadnim(int id)
        {
            var p = _ctx.Pregledi.Include(x => x.PrilozeniNalazi).FirstOrDefault(x => x.PregledID == id);
            if (p == null) return;
            _ctx.PrilozeniNalazi.RemoveRange(p.PrilozeniNalazi);
            _ctx.Pregledi.Remove(p);
        }

        public void Sacuvaj() => _ctx.SaveChanges();
    }
}
