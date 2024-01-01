using MySql.Data.MySqlClient;
using System.Resources;

namespace FormProject
{
    internal class DBConnection
    {
        MySqlConnection connection;

        public DBConnection()
        {
            ResourceManager rm = new ResourceManager("FormProject.Properties.Resources",
            typeof(DBConnection).Assembly);
            connection = new MySqlConnection(rm.GetString("connectData"));
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
