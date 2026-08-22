namespace KlasePodataka.Entiteti
{
    public class KorisnikKlasa
    {
        public int    KorisnikID { get; set; }
        public string Ime        { get; set; }
        public string Prezime    { get; set; }
        public string Email      { get; set; }
        public string Lozinka    { get; set; }
        public string Uloga      { get; set; }

        public string ImePrezime => Ime + " " + Prezime;
    }
}
