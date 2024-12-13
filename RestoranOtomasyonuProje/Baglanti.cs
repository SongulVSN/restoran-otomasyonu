using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranOtomasyonuProje
{
    internal class Baglanti
    {
        public SqlConnection Conn()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=SONGÜL;Initial Catalog=RestoranOtomasyon;Integrated Security=True");
            conn.Open();
            return conn;
        }
    }
}
