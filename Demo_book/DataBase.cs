using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_book
{
    internal class DataBase
    {
        SqlConnection conn = new SqlConnection(@"Data Source=KHOLODOVCOMP;Initial Catalog=bookmarket;Integrated Security=True");

        public void openDataBase()
        {
            conn.Open();
        }

        public void closeDataBase()
        {
            conn.Close();
        }

        public SqlConnection getConnection()
        {
            return conn;
        }
    }
}
