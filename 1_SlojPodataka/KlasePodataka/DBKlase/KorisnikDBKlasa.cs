using System.Linq;
using KlasePodataka.Entiteti;

namespace KlasePodataka.DBKlase
{
    public class KorisnikDBKlasa
    {
        private LekarskiPreglediKontekst _ctx;

        public KorisnikDBKlasa(LekarskiPreglediKontekst kontekst)
        {
            _ctx = kontekst;
        }

        public KorisnikKlasa DajPoKredencijalima(string email, string lozinka) =>
            _ctx.Korisnici.FirstOrDefault(k => k.Email == email && k.Lozinka == lozinka);
    }
}
