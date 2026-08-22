using System;
using System.Data.SqlClient;

namespace DBUtils
{
    public class KonekcijaKlasa
    {
        private SqlConnection _konekcija;
        private string _putanjaBaze;
        private string _nazivBaze;
        private string _nazivDBMSinstance;
        private string _stringKonekcije;

        public KonekcijaKlasa(string nazivDBMSInstance, string putanjaBaze, string nazivBaze)
        {
            _putanjaBaze = putanjaBaze;
            _nazivBaze = nazivBaze;
            _nazivDBMSinstance = nazivDBMSInstance;
            _stringKonekcije = "";
        }

        public KonekcijaKlasa(string noviStringKonekcije)
        {
            _putanjaBaze = "";
            _nazivBaze = "";
            _nazivDBMSinstance = "";
            _stringKonekcije = noviStringKonekcije;
        }

        private string DajStringKonekcije()
        {
            if (string.IsNullOrEmpty(_stringKonekcije))
            {
                if (string.IsNullOrEmpty(_putanjaBaze))
                {
                    return "Data Source=" + _nazivDBMSinstance +
                           ";Initial Catalog=" + _nazivBaze +
                           ";Integrated Security=True";
                }
                return "Data Source=.\\" + _nazivDBMSinstance +
                       ";AttachDbFilename=" + _putanjaBaze + "\\" + _nazivBaze +
                       ";Integrated Security=True;Connect Timeout=30;User Instance=True";
            }
            return _stringKonekcije;
        }

        public bool OtvoriKonekciju()
        {
            _konekcija = new SqlConnection();
            _konekcija.ConnectionString = DajStringKonekcije();
            try
            {
                _konekcija.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska pri otvaranju konekcije: " + ex.Message);
                return false;
            }
        }

        public SqlConnection DajKonekciju() => _konekcija;

        public void ZatvoriKonekciju()
        {
            if (_konekcija != null && _konekcija.State == System.Data.ConnectionState.Open)
            {
                _konekcija.Close();
                _konekcija.Dispose();
            }
        }
    }
}
