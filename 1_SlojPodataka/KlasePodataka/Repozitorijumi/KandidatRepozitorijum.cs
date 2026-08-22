using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.Repozitorijumi
{
    public class KandidatRepozitorijum : RepozitorijumKlasa<KandidatKlasa>
    {
        public KandidatRepozitorijum(LekarskiPreglediKontekst kontekst) : base(kontekst) { }

        public override IEnumerable<KandidatKlasa> DajSve() =>
            _skup.Include(k => k.Skola).Include(k => k.Profil)
                 .OrderBy(k => k.Prezime).ThenBy(k => k.Ime).ToList();

        public override KandidatKlasa DajPoId(int id) =>
            _skup.Include(k => k.Skola).Include(k => k.Profil)
                 .FirstOrDefault(k => k.KandidatID == id);

        public bool PostojiJMBG(string jmbg, int izuzmiId = 0) =>
            _skup.Any(k => k.JMBG == jmbg && k.KandidatID != izuzmiId);

        public override void Izmeni(KandidatKlasa k)
        {
            var e = _skup.Find(k.KandidatID);
            if (e == null) return;
            e.Ime           = k.Ime;
            e.Prezime       = k.Prezime;
            e.JMBG          = k.JMBG;
            e.DatumRodjenja = k.DatumRodjenja;
            e.SkolaID       = k.SkolaID;
            e.ProfilID      = k.ProfilID;
            e.VrstaUpisa    = k.VrstaUpisa;
            e.Aktivan       = k.Aktivan;
        }
    }
}
