using FormProject.Model;
using MySql.Data.MySqlClient;
using System.Resources;

namespace FormProject
{
    internal class DBConnection
    {
        MySqlConnection connection;
        string connectionString = "server={0};port={1};username={2};password={3};database={4}";

        //public DBConnection()
        //{
        //    ResourceManager rm = new ResourceManager("FormProject.Properties.Resources",
        //    typeof(DBConnection).Assembly);
        //    connection = new MySqlConnection(rm.GetString("connectData"));
        //}

        public DBConnection()
        {
            JsonParsing.ServerData serverData = JsonParsing.ParseServer();
            connection = new MySqlConnection(string.Format(connectionString, serverData.Server, serverData.Port,
                serverData.Username, serverData.Password, serverData.Database));
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
