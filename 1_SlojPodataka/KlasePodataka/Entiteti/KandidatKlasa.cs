using System;
using System.Collections.Generic;

namespace KlasePodataka.Entiteti
{
    public class KandidatKlasa
    {
        public int      KandidatID     { get; set; }
        public string   Ime            { get; set; }
        public string   Prezime        { get; set; }
        public string   JMBG           { get; set; }
        public DateTime DatumRodjenja  { get; set; }
        public int      SkolaID        { get; set; }
        public int      ProfilID       { get; set; }
        public string   VrstaUpisa     { get; set; }
        public bool     Aktivan        { get; set; }

        public virtual SrednjaSkolaKlasa     Skola  { get; set; }
        public virtual ObrazovniProfilKlasa  Profil { get; set; }
        public virtual ICollection<LekarskiPregledKlasa> Pregledi { get; set; }

        public string ImePrezime => Ime + " " + Prezime;
        public int    Starost    => DateTime.Now.Year - DatumRodjenja.Year -
                                    (DateTime.Now.DayOfYear < DatumRodjenja.DayOfYear ? 1 : 0);
    }
}
