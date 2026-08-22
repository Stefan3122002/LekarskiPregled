using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KorisnickiInterfejs.Models
{
    public class KandidatViewModel
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

        // Za prikaz u tabeli
        public string SkolaNaziv  { get; set; }
        public string ProfilNaziv { get; set; }
        public string ProfilSifra { get; set; }

        // Dropdown liste
        public IEnumerable<SelectListItem> SkoleSeznam   { get; set; }
        public IEnumerable<SelectListItem> ProfiliSeznam { get; set; }

        public IEnumerable<SelectListItem> VrsteUpisaSeznam => new List<SelectListItem>
        {
            new SelectListItem { Value = "Redovan upis",                        Text = "Redovan upis" },
            new SelectListItem { Value = "Upis po posebnim uslovima",           Text = "Upis po posebnim uslovima" },
            new SelectListItem { Value = "Prijem sportiste",                    Text = "Prijem sportiste" },
            new SelectListItem { Value = "Prijem po afirmativnoj mjeri",        Text = "Prijem po afirmativnoj mjeri" }
        };
    }
}
