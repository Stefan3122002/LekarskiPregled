using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KlasePodataka.Entiteti
{
    public class KandidatKlasa
    {
        public int KandidatID { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Ime")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "JMBG je obavezan.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG mora imati tacno 13 cifara.")]
        [Display(Name = "JMBG")]
        public string JMBG { get; set; }

        [Required(ErrorMessage = "Datum rodjenja je obavezan.")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rodjenja")]
        public DateTime DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Srednja skola je obavezna.")]
        [Display(Name = "Srednja skola")]
        public int SkolaID { get; set; }

        [Required(ErrorMessage = "Obrazovni profil je obavezan.")]
        [Display(Name = "Obrazovni profil")]
        public int ProfilID { get; set; }

        [Required(ErrorMessage = "Vrsta upisa je obavezna.")]
        [StringLength(50)]
        [Display(Name = "Vrsta upisa")]
        public string VrstaUpisa { get; set; }

        [Display(Name = "Prijava aktivna")]
        public bool Aktivan { get; set; }

        public virtual SrednjaSkolaKlasa     Skola  { get; set; }
        public virtual ObrazovniProfilKlasa  Profil { get; set; }
        public virtual ICollection<LekarskiPregledKlasa> Pregledi { get; set; }

        public string ImePrezime => Ime + " " + Prezime;
        public int    Starost    => DateTime.Now.Year - DatumRodjenja.Year -
                                    (DateTime.Now.DayOfYear < DatumRodjenja.DayOfYear ? 1 : 0);
    }
}
