using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KlasePodataka.Entiteti
{
    public class LekarskiPregledKlasa
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

        [Required(ErrorMessage = "Broj lekara je obavezan.")]
        [Range(1, 10, ErrorMessage = "Broj lekara u komisiji mora biti izmedju 1 i 10.")]
        [Display(Name = "Broj lekara u komisiji")]
        public int BrojLekaraUKomisiji { get; set; }

        [Display(Name = "Pregled zavrsen")]
        public bool Zavrsen { get; set; }

        [StringLength(500)]
        [Display(Name = "Napomena")]
        public string Napomena { get; set; }

        public int? KorisnikID { get; set; }

        public virtual KandidatKlasa Kandidat { get; set; }
        public virtual KorisnikKlasa Korisnik { get; set; }
        public virtual ICollection<PrilozeniNalazKlasa> PrilozeniNalazi { get; set; }
    }
}
