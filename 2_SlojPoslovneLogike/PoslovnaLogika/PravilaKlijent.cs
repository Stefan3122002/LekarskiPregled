using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace PoslovnaLogika
{

    public class PravilaKlijent
    {
        private readonly string _bazaUrl;
        private static readonly HttpClient _http = new HttpClient();

        public PravilaKlijent()
        {
            _bazaUrl = "http://localhost:55100/api/pravila/";
        }

        private T Daj<T>(string ruta)
        {
            try
            {
                string json = _http.GetStringAsync(_bazaUrl + ruta).Result;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("Greska pri pozivu web servisa: " + ex.Message);
            }
        }

        public int    JmbgDuzina()       => Daj<int>("jmbgDuzina");
        public string PorukaJmbg()       => Daj<string>("porukaJmbg");
        public int    MinStarostGodina() => Daj<int>("minStarostGodina");
        public string PorukaMinStarost() => Daj<string>("porukaMinStarost");
        public int    MaxStarostGodina() => Daj<int>("maxStarostGodina");
        public string PorukaMaxStarost() => Daj<string>("porukaMaxStarost");

        public int    MinUstanovaDuzina()        => Daj<int>("minUstanovaDuzina");
        public string PorukaMinUstanova()         => Daj<string>("porukaMinUstanova");
        public int    MinAdresaDuzina()           => Daj<int>("minAdresaDuzina");
        public string PorukaMinAdresa()           => Daj<string>("porukaMinAdresa");
        public int    MinTrajanjeMinuta()         => Daj<int>("minTrajanjeMinuta");
        public int    MaxTrajanjeMinuta()         => Daj<int>("maxTrajanjeMinuta");
        public string PorukaTrajanje()            => Daj<string>("porukaTrajanje");
        public int    MinBrojLekara()             => Daj<int>("minBrojLekara");
        public int    MaxBrojLekara()             => Daj<int>("maxBrojLekara");
        public string PorukaBrojLekara()          => Daj<string>("porukaBrojLekara");
        public bool   NapomenaObaveznaZaZavrsen() => Daj<bool>("napomenaObaveznaZaZavrsen");
        public string PorukaNapomenaZavrsen()     => Daj<string>("porukaNapomenaZavrsen");

        public int    MinVrstaNalazaDuzina() => Daj<int>("minVrstaNalazaDuzina");
        public string PorukaMinVrstaNalaza() => Daj<string>("porukaMinVrstaNalaza");
        public bool   NapomenaObaveznaZaNeuredan() => Daj<bool>("napomenaObaveznaZaNeuredan");
        public string PorukaNapomenaNeuredan()     => Daj<string>("porukaNapomenaNeuredan");

        public ProfilSaDodatnimPregledimaDto[] DajProfileSaDodatnimPregledima() =>
            Daj<ProfilSaDodatnimPregledimaDto[]>("profiliSaDodatnimPregledima");
    }

    public class ProfilSaDodatnimPregledimaDto
    {
        public string   Sifra          { get; set; }
        public string   Naziv          { get; set; }
        public string[] PotrebniNalazi { get; set; }
    }
}
