using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace QLTS.Sql
{
    class Connection
    {
        public static string stringConnection = ConfigurationManager.ConnectionStrings["QLTraSuaDB"].ConnectionString;
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
