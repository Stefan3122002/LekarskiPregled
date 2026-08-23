using System;

namespace KlasePodataka.Entiteti
{
    // Nalaz dodatnog specijalistickog pregleda prilozen uz LekarskiPregled.
    // VrstaNalaza se uporedjuje sa spiskom "PotrebniNalazi" iz eksternog
    // XML/JSON parametra (PoslovnaLogika.PravilaKlijent) da bi se utvrdilo
    // da li je lekarsko uvjerenje kandidata potpuno i validno.
    public class PrilozeniNalazKlasa
    {
        public int      NalazID     { get; set; }
        public int      PregledID   { get; set; }
        public string   VrstaNalaza { get; set; }
        public DateTime DatumNalaza { get; set; }
        public string   Ustanova    { get; set; }
        public bool     Uredan      { get; set; }
        public string   Napomena    { get; set; }

        public virtual LekarskiPregledKlasa Pregled { get; set; }
    }
}
