using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FormProject
{
    internal class DBFunctions
    {
        DBConnection connectorToDb;
        DataTable table;
        MySqlDataAdapter cursor;

        public DBFunctions()
        {
            connectorToDb = new DBConnection();
            cursor = new MySqlDataAdapter();
        }

        public bool IsPassCorrect(string login, string password)
        {
            table = new DataTable();
            connectorToDb.OpenConnection();
            MySqlCommand registered = new MySqlCommand("select pass from users " +
                "where login = @uLogin limit 1;", connectorToDb.GetConnection());
            registered.Parameters.AddWithValue("@uLogin", login);
            object storedPass = registered.ExecuteScalar();
            try
            {
                return storedPass.ToString() == password;
            }
            catch (NullReferenceException) { return false; }
        }

        public bool IsRegistered(string login)
        {
            try
            {
                table = new DataTable();
                connectorToDb.OpenConnection();
                MySqlCommand registered = new MySqlCommand("select * from users " +
                    "where login = @uLogin;", connectorToDb.GetConnection());
                registered.Parameters.AddWithValue("@uLogin", login);
                cursor.SelectCommand = registered;
                cursor.Fill(table);
                connectorToDb.CloseConnection();
                return table.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsRegisered");
                Console.WriteLine(ex.ToString());
                return true;
            }
        }

        public bool Register(string login, string password) 
        {
            try
            {
                connectorToDb.OpenConnection();
                bool users = AddToUsersTable(login, password);
                connectorToDb.CloseConnection();
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Register");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private void DeleteStats(int statsID)
        {
            MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @sID", connectorToDb.GetConnection());
            delete.Parameters.AddWithValue("@sID", statsID);
            delete.ExecuteNonQuery();
        }

        private void DeleteProfile(int profileID)
        {
            MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @pID", connectorToDb.GetConnection());
            delete.Parameters.AddWithValue("@pID", profileID);
            delete.ExecuteNonQuery();
        }

        private bool AddToUsersTable(string login, string password)
        {
            int statsID = AddToStatsTable();
            int profileID = AddToProfilesTable();
            if (statsID == 0 | profileID == 0) { 
                if (statsID != 0) { DeleteStats(statsID); }
                if (profileID != 0) { DeleteProfile(profileID); }
                return false; 
            }
            try
            {
                string insertLogPass = "insert into users (login, pass, idStats, idProfile) " +
                    "values (@uLogin, @uPass, @uStatsId, @uProfileId);";
                MySqlCommand register = new MySqlCommand(insertLogPass, connectorToDb.GetConnection());
                register.Parameters.AddWithValue("@uLogin", login);
                register.Parameters.AddWithValue("@uPass", password);
                register.Parameters.AddWithValue("@uStatsId", statsID);
                register.Parameters.AddWithValue("@uProfileId", profileID);
                register.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Users");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private int AddToProfilesTable()
        {
            try
            {
                MySqlCommand idProfileInsert = new MySqlCommand("insert into profiles (about) values ('');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from profiles;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                return Convert.ToInt32(currentID.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Profiles");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        private int AddToStatsTable()
        {
            try
            {
                MySqlCommand idProfileInsert = new MySqlCommand("insert into stats (solved, unsolved, level) values ('', '', 0);", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from stats;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                return Convert.ToInt32(currentID.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Stats");
                Console.WriteLine(ex.ToString());
                return 0;
            }
        }

        private int IDByLog(string login)
        {
            string getUserId = "select idUser from users where login = @uLogin;";
            MySqlCommand commandID = new MySqlCommand(getUserId, connectorToDb.GetConnection());
            commandID.Parameters.AddWithValue("@uLogin", login);
            object userID = commandID.ExecuteScalar();
            if (userID == null) { return 0; }
            return (int)userID;
        }
    }
}
