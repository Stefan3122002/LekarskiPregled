using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.Repozitorijumi
{
    public class LekarskiPregledRepozitorijum : RepozitorijumKlasa<LekarskiPregledKlasa>, ILekarskiPregledRepozitorijum
    {
        public LekarskiPregledRepozitorijum(LekarskiPreglediKontekst kontekst) : base(kontekst) { }

        public override IEnumerable<LekarskiPregledKlasa> DajSve() =>
            _skup.Include(p => p.Kandidat.Skola)
                 .Include(p => p.Kandidat.Profil)
                 .OrderByDescending(p => p.DatumPregleda).ToList();

        public override LekarskiPregledKlasa DajPoId(int id) =>
            _skup.Include(p => p.Kandidat.Skola)
                 .Include(p => p.Kandidat.Profil)
                 .FirstOrDefault(p => p.PregledID == id);

        public IEnumerable<LekarskiPregledKlasa> DajSaFilterom(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return DajSve();

            string f = filter.Trim().ToLower();
            return _skup
                .Include(p => p.Kandidat.Skola)
                .Include(p => p.Kandidat.Profil)
                .Where(p => p.Kandidat.Ime.ToLower().Contains(f) ||
                            p.Kandidat.Prezime.ToLower().Contains(f) ||
                            p.ZdravstvenaUstanova.ToLower().Contains(f))
                .OrderByDescending(p => p.DatumPregleda)
                .ToList();
        }

        public LekarskiPregledKlasa DajSaNalazima(int id) =>
            _skup
                .Include(p => p.Kandidat.Skola)
                .Include(p => p.Kandidat.Profil)
                .Include(p => p.PrilozeniNalazi)
                .FirstOrDefault(p => p.PregledID == id);

        public override void Izmeni(LekarskiPregledKlasa p)
        {
            var e = _skup.Find(p.PregledID);
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
            var p = _skup.Include(x => x.PrilozeniNalazi).FirstOrDefault(x => x.PregledID == id);
            if (p == null) return;
            _ctx.PrilozeniNalazi.RemoveRange(p.PrilozeniNalazi);
            _skup.Remove(p);
        }
    }
}
