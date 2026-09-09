using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.DBKlase
{
    public class PrilozeniNalazDBKlasa
    {
        private LekarskiPreglediKontekst _ctx;

        public PrilozeniNalazDBKlasa(LekarskiPreglediKontekst kontekst)
        {
            _ctx = kontekst;
        }

        public IEnumerable<PrilozeniNalazKlasa> DajSve() =>
            _ctx.PrilozeniNalazi.Include(n => n.Pregled.Kandidat)
                 .OrderByDescending(n => n.DatumNalaza).ToList();

        public PrilozeniNalazKlasa DajPoId(int id) =>
            _ctx.PrilozeniNalazi.Include(n => n.Pregled.Kandidat)
                 .FirstOrDefault(n => n.NalazID == id);

        public IEnumerable<PrilozeniNalazKlasa> DajZaPregled(int pregledId) =>
            _ctx.PrilozeniNalazi.Where(n => n.PregledID == pregledId).OrderBy(n => n.VrstaNalaza).ToList();

        public void Dodaj(PrilozeniNalazKlasa n) => _ctx.PrilozeniNalazi.Add(n);

        public void Izmeni(PrilozeniNalazKlasa n)
        {
            var e = _ctx.PrilozeniNalazi.Find(n.NalazID);
            if (e == null) return;
            e.PregledID   = n.PregledID;
            e.VrstaNalaza = n.VrstaNalaza;
            e.DatumNalaza = n.DatumNalaza;
            e.Ustanova    = n.Ustanova;
            e.Uredan      = n.Uredan;
            e.Napomena    = n.Napomena;
        }

        public void Obrisi(int id)
        {
            var e = _ctx.PrilozeniNalazi.Find(id);
            if (e != null) _ctx.PrilozeniNalazi.Remove(e);
        }

        public void Sacuvaj() => _ctx.SaveChanges();
    }
}
