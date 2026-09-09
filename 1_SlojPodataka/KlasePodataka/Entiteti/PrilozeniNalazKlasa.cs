using System;
using System.ComponentModel.DataAnnotations;

namespace KlasePodataka.Entiteti
{
    public class PrilozeniNalazKlasa
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

        public virtual LekarskiPregledKlasa Pregled { get; set; }
    }
}
