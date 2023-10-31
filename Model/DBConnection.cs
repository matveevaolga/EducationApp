using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal class DBConnection
    {
        MySqlConnection connection;

        public DBConnection()
        {
            connection = new MySqlConnection("server=localhost;port=3306;username=root;password=t-tAq$C45qw45;database=educationappdb");
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed) { connection.Open(); }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) { connection.Close();}
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
