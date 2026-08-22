using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KorisnickiInterfejs.Models
{
    public class PrilozeniNalazViewModel
    {
        public int NalazID { get; set; }

        [Required(ErrorMessage = "Lekarski pregled je obavezan.")]
        [Display(Name = "Lekarski pregled")]
        public int PregledID { get; set; }

        [Required(ErrorMessage = "Vrsta nalaza je obavezna.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Vrsta nalaza mora imati od 3 do 150 karaktera.")]
        [Display(Name = "Vrsta nalaza")]
        public string VrstaNalaza { get; set; }

        [Required(ErrorMessage = "Datum nalaza je obavezan.")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum nalaza")]
        public DateTime DatumNalaza { get; set; }

        [Required(ErrorMessage = "Ustanova je obavezna.")]
        [StringLength(200)]
        [Display(Name = "Ustanova koja je izdala nalaz")]
        public string Ustanova { get; set; }

        [Display(Name = "Nalaz uredan")]
        public bool Uredan { get; set; }

        [StringLength(500)]
        [Display(Name = "Napomena")]
        public string Napomena { get; set; }

        // Za prikaz u tabeli
        public string PregledKandidatImePrezime { get; set; }
        public DateTime PregledDatum             { get; set; }

        // Dropdown lista pregleda
        public IEnumerable<SelectListItem> PregledSeznam { get; set; }

        // Lista uobicajenih vrsta nalaza za dropdown (odgovaraju spisku iz eksternog pravila)
        public IEnumerable<SelectListItem> VrsteNalazaSeznam => new List<SelectListItem>
        {
            new SelectListItem { Value = "Pregled oftalmologa",              Text = "Pregled oftalmologa" },
            new SelectListItem { Value = "Pregled otorinolaringologa (ORL)", Text = "Pregled otorinolaringologa (ORL)" },
            new SelectListItem { Value = "Pregled dermatologa",              Text = "Pregled dermatologa" },
            new SelectListItem { Value = "Pregled neuropsihijatra",          Text = "Pregled neuropsihijatra" },
            new SelectListItem { Value = "Pregled kardiologa",               Text = "Pregled kardiologa" },
            new SelectListItem { Value = "Pregled ortopeda",                 Text = "Pregled ortopeda" },
            new SelectListItem { Value = "Ostalo",                          Text = "Ostalo" }
        };
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Email nije u ispravnom formatu.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Lozinka { get; set; }
    }
}
