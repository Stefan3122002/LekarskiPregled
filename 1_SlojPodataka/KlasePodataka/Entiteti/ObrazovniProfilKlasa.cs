using System.Collections.Generic;

namespace KlasePodataka.Entiteti
{

    public class ObrazovniProfilKlasa
    {
        public int    ProfilID          { get; set; }
        public string Sifra             { get; set; }
        public string Naziv             { get; set; }
        public string Opis              { get; set; }
        public int    NivoZahtevnosti  { get; set; }

        public virtual ICollection<KandidatKlasa> Kandidati { get; set; }
    }
}
