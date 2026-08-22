using System;
using System.Data;
using System.Data.SqlClient;

namespace DBUtils
{
    public class TabelaKlasa
    {
        private string _stringKonekcije;

        public TabelaKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public DataTable IzvrsiUpit(string sql, SqlParameter[] parametri = null)
        {
            DataTable tabela = new DataTable();
            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();
                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    if (parametri != null)
                        komanda.Parameters.AddRange(parametri);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(komanda))
                    {
                        adapter.Fill(tabela);
                    }
                }
            }
            return tabela;
        }

        public void IzvrsiKomandu(string sql, SqlParameter[] parametri = null)
        {
            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();
                using (SqlCommand komanda = new SqlCommand(sql, konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    if (parametri != null)
                        komanda.Parameters.AddRange(parametri);
                    komanda.ExecuteNonQuery();
                }
            }
        }
    }
}
