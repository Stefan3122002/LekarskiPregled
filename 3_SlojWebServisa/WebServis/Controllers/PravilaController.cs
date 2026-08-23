using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;

namespace WebServis.Controllers
{
    // REST Web API kontroler za poslovna pravila - cita parametre iz eksterne
    // XML konfiguracione datoteke (Pravila.xml), ukljucujuci spisak obrazovnih
    // profila koji zahtijevaju dodatne specijalisticke preglede.
    [RoutePrefix("api/pravila")]
    public class PravilaController : ApiController
    {
        private static XmlDocument _xml;
        private static readonly object _lock = new object();

        private XmlDocument DajXml()
        {
            if (_xml == null)
            {
                lock (_lock)
                {
                    if (_xml == null)
                    {
                        _xml = new XmlDocument();
                        string putanja = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XML", "Pravila.xml");
                        _xml.Load(putanja);
                    }
                }
            }
            return _xml;
        }

        private string CitajVrednost(string tag)
        {
            XmlNode node = DajXml().SelectSingleNode($"/Pravila/{tag}");
            return node?.InnerText ?? string.Empty;
        }

        // ======= KANDIDAT =======

        [HttpGet, Route("jmbgDuzina")]
        public IHttpActionResult JmbgDuzina() => Ok(int.Parse(CitajVrednost("JMBGDuzina")));

        [HttpGet, Route("porukaJmbg")]
        public IHttpActionResult PorukaJmbg() => Ok(CitajVrednost("PorukaJMBG"));

        [HttpGet, Route("minStarostGodina")]
        public IHttpActionResult MinStarostGodina() => Ok(int.Parse(CitajVrednost("MinStarostGodina")));

        [HttpGet, Route("porukaMinStarost")]
        public IHttpActionResult PorukaMinStarost() => Ok(CitajVrednost("PorukaMinStarost"));

        [HttpGet, Route("maxStarostGodina")]
        public IHttpActionResult MaxStarostGodina() => Ok(int.Parse(CitajVrednost("MaxStarostGodina")));

        [HttpGet, Route("porukaMaxStarost")]
        public IHttpActionResult PorukaMaxStarost() => Ok(CitajVrednost("PorukaMaxStarost"));

        // ======= LEKARSKI PREGLED =======

        [HttpGet, Route("minUstanovaDuzina")]
        public IHttpActionResult MinUstanovaDuzina() => Ok(int.Parse(CitajVrednost("MinUstanovaDuzina")));

        [HttpGet, Route("porukaMinUstanova")]
        public IHttpActionResult PorukaMinUstanova() => Ok(CitajVrednost("PorukaMinUstanova"));

        [HttpGet, Route("minAdresaDuzina")]
        public IHttpActionResult MinAdresaDuzina() => Ok(int.Parse(CitajVrednost("MinAdresaDuzina")));

        [HttpGet, Route("porukaMinAdresa")]
        public IHttpActionResult PorukaMinAdresa() => Ok(CitajVrednost("PorukaMinAdresa"));

        [HttpGet, Route("minTrajanjeMinuta")]
        public IHttpActionResult MinTrajanjeMinuta() => Ok(int.Parse(CitajVrednost("MinTrajanjeMinuta")));

        [HttpGet, Route("maxTrajanjeMinuta")]
        public IHttpActionResult MaxTrajanjeMinuta() => Ok(int.Parse(CitajVrednost("MaxTrajanjeMinuta")));

        [HttpGet, Route("porukaTrajanje")]
        public IHttpActionResult PorukaTrajanje() => Ok(CitajVrednost("PorukaTrajanje"));

        [HttpGet, Route("minBrojLekara")]
        public IHttpActionResult MinBrojLekara() => Ok(int.Parse(CitajVrednost("MinBrojLekara")));

        [HttpGet, Route("maxBrojLekara")]
        public IHttpActionResult MaxBrojLekara() => Ok(int.Parse(CitajVrednost("MaxBrojLekara")));

        [HttpGet, Route("porukaBrojLekara")]
        public IHttpActionResult PorukaBrojLekara() => Ok(CitajVrednost("PorukaBrojLekara"));

        [HttpGet, Route("napomenaObaveznaZaZavrsen")]
        public IHttpActionResult NapomenaObaveznaZaZavrsen() => Ok(bool.Parse(CitajVrednost("NapomenaObaveznaZaZavrsen")));

        [HttpGet, Route("porukaNapomenaZavrsen")]
        public IHttpActionResult PorukaNapomenaZavrsen() => Ok(CitajVrednost("PorukaNapomenaZavrsen"));

        // ======= PRILOZENI NALAZ =======

        [HttpGet, Route("minVrstaNalazaDuzina")]
        public IHttpActionResult MinVrstaNalazaDuzina() => Ok(int.Parse(CitajVrednost("MinVrstaNalazaDuzina")));

        [HttpGet, Route("porukaMinVrstaNalaza")]
        public IHttpActionResult PorukaMinVrstaNalaza() => Ok(CitajVrednost("PorukaMinVrstaNalaza"));

        [HttpGet, Route("napomenaObaveznaZaNeuredan")]
        public IHttpActionResult NapomenaObaveznaZaNeuredan() => Ok(bool.Parse(CitajVrednost("NapomenaObaveznaZaNeuredan")));

        [HttpGet, Route("porukaNapomenaNeuredan")]
        public IHttpActionResult PorukaNapomenaNeuredan() => Ok(CitajVrednost("PorukaNapomenaNeuredan"));

        // ======= POSLOVNO PRAVILO: profili sa dodatnim pregledima =======

        // Vraca spisak obrazovnih profila koji zahtijevaju dodatne specijalisticke
        // preglede, sa spiskom potrebnih nalaza za svaki profil.
        [HttpGet, Route("profiliSaDodatnimPregledima")]
        public IHttpActionResult ProfiliSaDodatnimPregledima()
        {
            XmlNodeList cvorovi = DajXml().SelectNodes("/Pravila/ProfiliSaDodatnimPregledima/Profil");
            var lista = new List<ProfilSaDodatnimPregledimaDto>();

            foreach (XmlNode profilNode in cvorovi)
            {
                var nalazi = profilNode.SelectNodes("PotrebniNalazi/Nalaz")
                    .Cast<XmlNode>()
                    .Select(n => n.InnerText)
                    .ToArray();

                lista.Add(new ProfilSaDodatnimPregledimaDto
                {
                    Sifra          = profilNode["Sifra"].InnerText,
                    Naziv          = profilNode["Naziv"].InnerText,
                    PotrebniNalazi = nalazi
                });
            }

            return Ok(lista);
        }
    }

    public class ProfilSaDodatnimPregledimaDto
    {
        public string   Sifra          { get; set; }
        public string   Naziv          { get; set; }
        public string[] PotrebniNalazi { get; set; }
    }
}
