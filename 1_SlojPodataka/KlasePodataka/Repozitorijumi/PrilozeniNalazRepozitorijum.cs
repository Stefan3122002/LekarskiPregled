using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.Repozitorijumi
{
    public class PrilozeniNalazRepozitorijum : RepozitorijumKlasa<PrilozeniNalazKlasa>
    {
        public PrilozeniNalazRepozitorijum(LekarskiPreglediKontekst kontekst) : base(kontekst) { }

        public override IEnumerable<PrilozeniNalazKlasa> DajSve() =>
            _skup.Include(n => n.Pregled.Kandidat)
                 .OrderByDescending(n => n.DatumNalaza).ToList();

        public override PrilozeniNalazKlasa DajPoId(int id) =>
            _skup.Include(n => n.Pregled.Kandidat)
                 .FirstOrDefault(n => n.NalazID == id);

        public IEnumerable<PrilozeniNalazKlasa> DajZaPregled(int pregledId) =>
            _skup.Where(n => n.PregledID == pregledId).OrderBy(n => n.VrstaNalaza).ToList();

        public override void Izmeni(PrilozeniNalazKlasa n)
        {
            var e = _skup.Find(n.NalazID);
            if (e == null) return;
            e.PregledID   = n.PregledID;
            e.VrstaNalaza = n.VrstaNalaza;
            e.DatumNalaza = n.DatumNalaza;
            e.Ustanova    = n.Ustanova;
            e.Uredan      = n.Uredan;
            e.Napomena    = n.Napomena;
        }
    }
}
