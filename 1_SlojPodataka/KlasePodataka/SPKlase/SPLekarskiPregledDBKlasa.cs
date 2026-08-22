using System;
using System.Data;
using System.Data.SqlClient;
using DBUtils;

namespace KlasePodataka.SPKlase
{
    // Klasa koja poziva uskladistene procedure za parametarsku stampu lekarskih pregleda
    public class SPLekarskiPregledDBKlasa
    {
        private string _stringKonekcije;

        public SPLekarskiPregledDBKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public DataTable DajPreglededZaStampu(DateTime? datumOd, DateTime? datumDo, int? profilID)
        {
            TabelaKlasa tabela = new TabelaKlasa(_stringKonekcije);

            SqlParameter[] parametri = {
                new SqlParameter("@DatumOd", (object)datumOd ?? DBNull.Value),
                new SqlParameter("@DatumDo", (object)datumDo ?? DBNull.Value),
                new SqlParameter("@ProfilID", (object)profilID ?? DBNull.Value)
            };

            return tabela.IzvrsiUpit("spDajPreglededZaStampu", parametri);
        }
    }
}
