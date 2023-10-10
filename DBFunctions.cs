using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal class DBFunctions
    {
        DBConnection connection = new DBConnection();
        DataTable table = new DataTable();
        MySqlDataAdapter cursor = new MySqlDataAdapter();

        public bool IsRegistered(string login)
        {
            MySqlCommand registered = new MySqlCommand("SELECT * FROM `users`" +
                "where `login` = @uLogin", connection.GetConnection());
            registered.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            connection.OpenConnection();
            cursor.SelectCommand = registered;
            cursor.Fill(table);
            connection.CloseConnection();
            return table.Rows.Count > 0; 
        }

        public bool Register(string login, string password) 
        {
            Console.WriteLine(login);
            try
            {
                connection.OpenConnection();
                bool users = AddToUsersTable(login, password);
                connection.CloseConnection();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection to DB failed");
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool AddToUsersTable(string login, string password)
        {
            string idStatsString = AddToStatsTable(login);
            string idProfileString = AddToProfilesTable(login);
            int idProfile, idStats;
            if (!int.TryParse(idStatsString, out idStats) | !int.TryParse(idProfileString, out idProfile)) { return false; }
            string command = "insert into users (login, pass, idStats, idProfile) values (@uLogin, @uPass, @uStatsId, @uProfileId);";
            MySqlCommand commandInsert = new MySqlCommand(command, connection.GetConnection());
            commandInsert.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            commandInsert.Parameters.Add("@uPass", MySqlDbType.VarChar).Value = password;
            commandInsert.Parameters.Add("@uStatsId", MySqlDbType.VarChar).Value = idStats;
            commandInsert.Parameters.Add("@uProfileId", MySqlDbType.VarChar).Value = idProfile;
            commandInsert.ExecuteNonQuery();
        }

        private string AddToProfilesTable(string login)
        {
            MySqlCommand commandInsert = new MySqlCommand("insert into profiles (name) values (@uLogin);", connection.GetConnection());
            commandInsert.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            commandInsert.ExecuteNonQuery();
            MySqlCommand commandSelect = new MySqlCommand("select idProfile from profiles where name = @uLogin;", connection.GetConnection());
            commandSelect.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            object result = commandSelect.ExecuteScalar();
            if (result != null) { return Convert.ToString(result); }
            return "";
        }

        private string AddToStatsTable(string login)
        {
            MySqlCommand commandInsert = new MySqlCommand("insert into stats (username) values (@uLogin);", connection.GetConnection());
            commandInsert.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            commandInsert.ExecuteNonQuery();
            MySqlCommand commandSelect = new MySqlCommand("select idStats from profiles where username = @uLogin;", connection.GetConnection());
            commandSelect.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            object result = commandSelect.ExecuteScalar();
            if (result != null) { return Convert.ToString(result); }
            return ""; 
        }
    }
}
