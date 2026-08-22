using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.Repozitorijumi
{
    public class KorisnikRepozitorijum : RepozitorijumKlasa<KorisnikKlasa>
    {
        public KorisnikRepozitorijum(LekarskiPreglediKontekst kontekst) : base(kontekst) { }

        public KorisnikKlasa DajPoKredencijalima(string email, string lozinka) =>
            _skup.FirstOrDefault(k => k.Email == email && k.Lozinka == lozinka);

        public override void Izmeni(KorisnikKlasa k)
        {
            var e = _skup.Find(k.KorisnikID);
            if (e == null) return;
            e.Ime     = k.Ime;
            e.Prezime = k.Prezime;
            e.Email   = k.Email;
            e.Lozinka = k.Lozinka;
            e.Uloga   = k.Uloga;
        }
    }
}
