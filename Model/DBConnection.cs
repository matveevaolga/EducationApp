using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;

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
