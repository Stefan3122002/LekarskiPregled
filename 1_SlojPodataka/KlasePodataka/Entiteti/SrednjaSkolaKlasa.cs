using System;
using System.Collections.Generic;

namespace KlasePodataka.Entiteti
{
    public class SrednjaSkolaKlasa
    {
        public int      SkolaID         { get; set; }
        public string   Naziv           { get; set; }
        public string   Adresa          { get; set; }
        public string   Grad            { get; set; }
        public string   Telefon         { get; set; }
        public DateTime? DatumOsnivanja { get; set; }

        public virtual ICollection<KandidatKlasa> Kandidati { get; set; }
    }
}
