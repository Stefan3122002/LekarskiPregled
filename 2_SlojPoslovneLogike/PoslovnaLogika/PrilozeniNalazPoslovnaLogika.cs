using KlasePodataka.Entiteti;

namespace PoslovnaLogika
{
    public class PrilozeniNalazPoslovnaLogika
    {
        private PravilaKlijent _servis;

        public PrilozeniNalazPoslovnaLogika()
        {
            _servis = new PravilaKlijent();
        }

        public bool ValidirajNalaz(PrilozeniNalazKlasa n, out string poruka)
        {
            poruka = string.Empty;

            if (string.IsNullOrWhiteSpace(n.VrstaNalaza) || n.VrstaNalaza.Trim().Length < _servis.MinVrstaNalazaDuzina())
            {
                poruka = _servis.PorukaMinVrstaNalaza();
                return false;
            }

            if (!n.Uredan && _servis.NapomenaObaveznaZaNeuredan() && string.IsNullOrWhiteSpace(n.Napomena))
            {
                poruka = _servis.PorukaNapomenaNeuredan();
                return false;
            }

            return true;
        }
    }
}
