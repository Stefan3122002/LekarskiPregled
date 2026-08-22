using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace KorisnickiInterfejs.Models
{
    public class LekarskiPregledViewModel
    {
        public int PregledID { get; set; }

        [Required(ErrorMessage = "Kandidat je obavezan.")]
        [Display(Name = "Kandidat")]
        public int KandidatID { get; set; }

        [Required(ErrorMessage = "Datum pregleda je obavezan.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Datum pregleda")]
        public DateTime DatumPregleda { get; set; }

        [Required(ErrorMessage = "Zdravstvena ustanova je obavezna.")]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name = "Zdravstvena ustanova")]
        public string ZdravstvenaUstanova { get; set; }

        [Required(ErrorMessage = "Adresa ustanove je obavezna.")]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name = "Adresa ustanove")]
        public string AdresaUstanove { get; set; }

        [Required(ErrorMessage = "Trajanje pregleda je obavezno.")]
        [Range(5, 180, ErrorMessage = "Trajanje mora biti izmedju 5 i 180 minuta.")]
        [Display(Name = "Trajanje pregleda (minuta)")]
        public int TrajanjePregledaMinuta { get; set; }

        [Required(ErrorMessage = "Broj ljekara je obavezan.")]
        [Range(1, 10, ErrorMessage = "Broj ljekara u komisiji mora biti izmedju 1 i 10.")]
        [Display(Name = "Broj ljekara u komisiji")]
        public int BrojLekaraUKomisiji { get; set; }

        [Display(Name = "Pregled zavrsen")]
        public bool Zavrsen { get; set; }

        [StringLength(500)]
        [Display(Name = "Napomena")]
        public string Napomena { get; set; }

        // Za prikaz u tabeli
        public string KandidatImePrezime { get; set; }
        public string SkolaNaziv         { get; set; }
        public string ProfilNaziv        { get; set; }

        // Dropdown lista kandidata
        public IEnumerable<SelectListItem> KandidatiSeznam { get; set; }
    }

    public class StampaLekarskiPregledViewModel
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Datum od")]
        public DateTime? DatumOd { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Datum do")]
        public DateTime? DatumDo { get; set; }

        [Display(Name = "Obrazovni profil")]
        public int? ProfilID { get; set; }

        public IEnumerable<SelectListItem> ProfiliSeznam { get; set; }

        public System.Data.DataTable Rezultati { get; set; }
    }
}
