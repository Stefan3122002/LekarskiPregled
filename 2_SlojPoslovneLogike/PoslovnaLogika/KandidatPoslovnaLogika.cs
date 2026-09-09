using System;
using KlasePodataka.Entiteti;

namespace PoslovnaLogika
{
    public class KandidatPoslovnaLogika
    {
        private PravilaKlijent _servis;

        public KandidatPoslovnaLogika()
        {
            _servis = new PravilaKlijent();
        }

        public bool ValidirajKandidata(KandidatKlasa k, out string poruka)
        {
            poruka = string.Empty;

            if (string.IsNullOrWhiteSpace(k.JMBG) || k.JMBG.Trim().Length != _servis.JmbgDuzina())
            {
                poruka = _servis.PorukaJmbg();
                return false;
            }

            foreach (char c in k.JMBG.Trim())
            {
                if (!char.IsDigit(c))
                {
                    poruka = _servis.PorukaJmbg();
                    return false;
                }
            }

            int starost = DateTime.Now.Year - k.DatumRodjenja.Year -
                          (DateTime.Now.DayOfYear < k.DatumRodjenja.DayOfYear ? 1 : 0);

            if (starost < _servis.MinStarostGodina())
            {
                poruka = _servis.PorukaMinStarost();
                return false;
            }

            if (starost > _servis.MaxStarostGodina())
            {
                poruka = _servis.PorukaMaxStarost();
                return false;
            }

            return true;
        }
    }
}
