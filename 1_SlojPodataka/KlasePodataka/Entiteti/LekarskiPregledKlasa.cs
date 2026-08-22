using System;
using System.Collections.Generic;

namespace KlasePodataka.Entiteti
{
    public class LekarskiPregledKlasa
    {
        public int      PregledID              { get; set; }
        public int      KandidatID              { get; set; }
        public DateTime DatumPregleda           { get; set; }
        public string   ZdravstvenaUstanova     { get; set; }
        public string   AdresaUstanove          { get; set; }
        public int      TrajanjePregledaMinuta  { get; set; }
        public int      BrojLekaraUKomisiji     { get; set; }
        public bool     Zavrsen                 { get; set; }
        public string   Napomena                { get; set; }

        public virtual KandidatKlasa Kandidat { get; set; }
        public virtual ICollection<PrilozeniNalazKlasa> PrilozeniNalazi { get; set; }
    }
}
