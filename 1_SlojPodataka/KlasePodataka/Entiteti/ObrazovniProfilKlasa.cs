using System.Collections.Generic;

namespace KlasePodataka.Entiteti
{
    // Sifrarnik obrazovnih profila. Sifra se koristi za uparivanje sa
    // eksternim XML/JSON parametrom koji propisuje koji profili zahtijevaju
    // dodatne specijalisticke preglede (vidi PoslovnaLogika.PravilaKlijent).
    public class ObrazovniProfilKlasa
    {
        public int    ProfilID          { get; set; }
        public string Sifra             { get; set; }
        public string Naziv             { get; set; }
        public string Opis              { get; set; }
        public int    NivoZahtjevnosti  { get; set; }

        public virtual ICollection<KandidatKlasa> Kandidati { get; set; }
    }
}
